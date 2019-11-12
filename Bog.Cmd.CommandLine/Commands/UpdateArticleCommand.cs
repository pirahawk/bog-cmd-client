using Bog.Api.Domain.Models.Http;
using Bog.Cmd.CommandLine.Http;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.FileIO;
using Bog.Cmd.Domain.Values;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bog.Cmd.Common.Json;

namespace Bog.Cmd.CommandLine.Commands
{
    public class UpdateArticleCommand : IUpdateArticleCommand
    {
        private readonly BogHttpClient _client;
        private readonly IClientFileProvider _fileProvider;

        public UpdateArticleCommand(BogHttpClient client, IClientFileProvider fileProvider)
        {
            _client = client;
            _fileProvider = fileProvider;
        }

        public async Task UpdateArticle(string author, bool publish)
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
                Author = author ?? articleContext.Author,
                IsPublished = publish,
                BlogId = articleContext.BlogId
            };

            var updateArticleResponse = await _client.PutMessage(BogApiRouteValues.ARTICLE_GUID_FORMAT(articleContext.Id), articleRequest);

            if (!updateArticleResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"failed HTTP request: {updateArticleResponse.StatusCode}\n{updateArticleResponse.ReasonPhrase}");
            }

            var getArticleResponse = await _client.GetMessage(BogApiRouteValues.ARTICLE_GUID_FORMAT(articleContext.Id));

            if (!getArticleResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"failed HTTP request: {getArticleResponse.StatusCode}\n{getArticleResponse.ReasonPhrase}");
            }

            var blogContentTask = getArticleResponse.Content.ReadAsStringAsync();
            await _fileProvider.WriteMetaFile(MetaFileNameValues.ARTICLE, blogContentTask);
            var contents = await blogContentTask;
            contents = JsonUtility.Prettify<ArticleResponse>(contents);
            Console.WriteLine(contents);
        }
    }
}