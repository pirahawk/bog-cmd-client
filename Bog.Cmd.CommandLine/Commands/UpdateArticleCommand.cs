using Bog.Api.Domain.Models.Http;
using Bog.Cmd.CommandLine.Http;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Extensions;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bog.Cmd.CommandLine.Commands
{
    public class UpdateArticleCommand : IUpdateArticleCommand
    {
        private readonly BogHttpClient _client;
        private readonly IUpdateArticleContextWorkflow _updateArticleContextWorkflow;
        private readonly IGetArticleContextWorkflow _getArticleContextWorkflow;


        public UpdateArticleCommand(BogHttpClient client, IGetArticleContextWorkflow getArticleContextWorkflow, IUpdateArticleContextWorkflow updateArticleContextWorkflow)
        {
            _client = client;
            _getArticleContextWorkflow = getArticleContextWorkflow;
            _updateArticleContextWorkflow = updateArticleContextWorkflow;
        }

        public async Task UpdateArticle(string author = null, string title = null, string description = null, bool? publish = null)
        {
            var articleContext = await _getArticleContextWorkflow.GetArticleContext();
            if (articleContext == null)
            {
                return;
            }

            var articleRequest = new ArticleRequest
            {
                BlogId = articleContext.BlogId,
                Author = author,
                Title = title,
                Description = description,
                IsPublished = publish,
            };

            var updateArticleResponse = await _client.PutMessage(articleContext.GetSelfApiLink(), articleRequest);

            if (!updateArticleResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"failed HTTP request: {updateArticleResponse.StatusCode}\n{updateArticleResponse.ReasonPhrase}");
            }

            await _updateArticleContextWorkflow.GetAndUpdateArticleContext(articleContext);
        }
    }
}