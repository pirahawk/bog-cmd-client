using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Values;
using Bog.Cmd.CommandLine.Http;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace Bog.Cmd.CommandLine.Commands
{
    public class DeleteMetaTagsCommand : IDeleteMetaTagsCommand
    {
        private readonly BogHttpClient _client;
        private readonly IGetArticleContextWorkflow _getArticleContextWorkflow;

        public DeleteMetaTagsCommand(BogHttpClient client, IGetArticleContextWorkflow getArticleContextWorkflow)
        {
            _client = client;
            _getArticleContextWorkflow = getArticleContextWorkflow;
        }
        public async Task DeleteMetaTags(params string[] tagsToDelete)
        {
            var articleContext = await _getArticleContextWorkflow.GetArticleContext();
            if (articleContext == null)
            {
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