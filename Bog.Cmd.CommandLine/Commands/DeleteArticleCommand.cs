using Bog.Cmd.CommandLine.Http;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Extensions;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bog.Cmd.CommandLine.Commands
{
    public class DeleteArticleCommand : IDeleteArticleCommand
    {
        private readonly BogHttpClient _client;
        private readonly IUpdateArticleContextWorkflow _updateArticleContextWorkflow;
        private readonly IGetArticleContextWorkflow _getArticleContextWorkflow;

        public DeleteArticleCommand(BogHttpClient client,  IUpdateArticleContextWorkflow updateArticleContextWorkflow, IGetArticleContextWorkflow getArticleContextWorkflow)
        {
            _client = client;
            _updateArticleContextWorkflow = updateArticleContextWorkflow;
            _getArticleContextWorkflow = getArticleContextWorkflow;
        }

        public async Task MarkArticleAsDeleted()
        {
            var articleContext = await _getArticleContextWorkflow.GetArticleContext();
            if (articleContext == null)
            {
                return;
            }

            var deleteResponse = await _client.DeleteMessage(articleContext.GetSelfApiLink());

            if (!deleteResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"failed HTTP request: {deleteResponse.StatusCode}\n{deleteResponse.ReasonPhrase}");
            }

            await _updateArticleContextWorkflow.GetAndUpdateArticleContext(articleContext);
        }
    }
}