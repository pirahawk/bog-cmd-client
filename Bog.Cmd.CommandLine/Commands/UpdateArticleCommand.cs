using Bog.Api.Domain.Models.Http;
using Bog.Cmd.CommandLine.Http;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.FileIO;
using Bog.Cmd.Domain.Values;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bog.Cmd.Domain.Extensions;

namespace Bog.Cmd.CommandLine.Commands
{
    public class UpdateArticleCommand : IUpdateArticleCommand
    {
        private readonly BogHttpClient _client;
        private readonly IClientFileProvider _fileProvider;
        private readonly IUpdateArticleContextWorkflow _updateArticleContextWorkflow;

        public UpdateArticleCommand(BogHttpClient client, IClientFileProvider fileProvider, IUpdateArticleContextWorkflow updateArticleContextWorkflow)
        {
            _client = client;
            _fileProvider = fileProvider;
            _updateArticleContextWorkflow = updateArticleContextWorkflow;
        }

        public async Task UpdateArticle(string author = null, string title = null, string description = null, bool? publish = null)
        {
            if (!_fileProvider.CheckMetaFileExists(MetaFileNameValues.ARTICLE))
            {
                Console.Error.WriteLine("No Article exists in current context");
                return;
            }

            var articleContext = await _fileProvider.ReadMetaFile<ArticleResponse>(MetaFileNameValues.ARTICLE);

            if (articleContext == null)
            {
                Console.Error.WriteLine("Could not read article information from current context");
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