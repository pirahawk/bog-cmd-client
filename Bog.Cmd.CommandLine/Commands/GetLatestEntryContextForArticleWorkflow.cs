using System.Threading.Tasks;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Values;
using Bog.Cmd.CommandLine.Http;
using Bog.Cmd.Common.Json;
using Bog.Cmd.Domain.Extensions;

namespace Bog.Cmd.CommandLine.Commands
{
    public class GetLatestEntryContextForArticleWorkflow : IGetLatestEntryContextForArticleWorkflow
    {
        private readonly IGetArticleContextWorkflow _getArticleContextWorkflow;
        private readonly BogHttpClient _client;

        public GetLatestEntryContextForArticleWorkflow(IGetArticleContextWorkflow getArticleContextWorkflow, BogHttpClient client)
        {
            _getArticleContextWorkflow = getArticleContextWorkflow;
            _client = client;
        }

        public async Task<ArticleEntryResponse> GetLatestArticleEntryFromContext()
        {
            var articleContext = await _getArticleContextWorkflow.GetArticleContext();

            if (articleContext == null)
            {
                return null;
            }

            var getEntryLink =  articleContext.GetApiLink(LinkRelValueObject.ENTRY);

            if (string.IsNullOrWhiteSpace(getEntryLink))
            {
                return null;
            }

            var responseMessage = await _client.GetMessage(getEntryLink);

            if (!responseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            var entryResponseContents = await responseMessage.Content.ReadAsStringAsync();
            var articleEntry = JsonUtility.Deserialize<ArticleEntryResponse>(entryResponseContents);
            return articleEntry;
        }
    }
}