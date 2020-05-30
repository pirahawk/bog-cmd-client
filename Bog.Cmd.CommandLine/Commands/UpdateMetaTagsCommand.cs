using Bog.Api.Domain.Models.Http;
using Bog.Cmd.CommandLine.Http;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.FileIO;
using Bog.Cmd.Domain.Values;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bog.Api.Domain.Values;
using Bog.Cmd.Domain.Extensions;

namespace Bog.Cmd.CommandLine.Commands
{
    public class UpdateMetaTagsCommand : IUpdateMetaTagsCommand
    {
        private readonly BogHttpClient _client;
        private readonly IClientFileProvider _fileProvider;

        public UpdateMetaTagsCommand(BogHttpClient client, IClientFileProvider fileProvider)
        {
            _client = client;
            _fileProvider = fileProvider;
        }

        public async Task UpdateMetaTags(params string[] tags)
        {
            if (!_fileProvider.CheckMetaFileExists(MetaFileNameValues.ARTICLE))
            {
                await Console.Error.WriteLineAsync("No Article exists in current context");
                return;
            }

            var articleContext = await _fileProvider.ReadMetaFile<ArticleResponse>(MetaFileNameValues.ARTICLE);

            if (articleContext == null)
            {
                Console.Error.WriteLine("Could not read article information from current context");
                return;
            }

            var tagRequests = tags.Select(tag => new MetaTagRequest
            {
                Name = tag
            }).ToArray();

            var metaRequestLink = articleContext.GetApiLink(LinkRelValueObject.META_TAG);
            var response = await _client.PostMessage(metaRequestLink, tagRequests);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"failed HTTP request: {response.StatusCode}\n{response.ReasonPhrase}");
            }
        }
    }
}