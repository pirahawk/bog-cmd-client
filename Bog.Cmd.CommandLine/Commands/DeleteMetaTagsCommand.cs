using System;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Values;
using Bog.Cmd.CommandLine.Http;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Extensions;
using Bog.Cmd.Domain.FileIO;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Commands
{
    public class DeleteMetaTagsCommand : IDeleteMetaTagsCommand
    {
        private readonly BogHttpClient _client;
        private readonly IClientFileProvider _fileProvider;

        public DeleteMetaTagsCommand(BogHttpClient client, IClientFileProvider fileProvider)
        {
            _client = client;
            _fileProvider = fileProvider;
        }
        public async Task DeleteMetaTags(params string[] tagsToDelete)
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

            var tagRequests = tagsToDelete.Select(tag => new MetaTagRequest
            {
                Name = tag
            }).ToArray();

            var metaRequestLink = articleContext.GetApiLink(LinkRelValueObject.META_TAG);
            var response = await _client.DeleteMessage(metaRequestLink, tagRequests);
        }
    }
}