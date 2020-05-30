using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Values;
using Bog.Cmd.CommandLine.Http;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Extensions;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bog.Cmd.CommandLine.Commands
{
    public class UpdateMetaTagsCommand : IUpdateMetaTagsCommand
    {
        private readonly BogHttpClient _client;
        private readonly IGetArticleContextWorkflow _getArticleContextWorkflow;

        public UpdateMetaTagsCommand(BogHttpClient client, IGetArticleContextWorkflow getArticleContextWorkflow)
        {
            _client = client;
            _getArticleContextWorkflow = getArticleContextWorkflow;
        }

        public async Task UpdateMetaTags(params string[] tags)
        {
            var articleContext = await _getArticleContextWorkflow.GetArticleContext();
            if (articleContext == null)
            {
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