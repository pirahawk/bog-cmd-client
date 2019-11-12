using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bog.Api.Domain.Models.Http;
using Bog.Cmd.CommandLine.Http;
using Bog.Cmd.Common.Json;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.FileIO;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Commands
{
    public class DeleteArticleCommand : IDeleteArticleCommand
    {
        private readonly BogHttpClient _client;
        private readonly IClientFileProvider _fileProvider;

        public DeleteArticleCommand(BogHttpClient client,  IClientFileProvider fileProvider)
        {
            _client = client;
            _fileProvider = fileProvider;
        }

        public async Task MarkArticleAsDeleted()
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

            var deleteResponse = await _client.DeleteMessage(BogApiRouteValues.ARTICLE_GUID_FORMAT(articleContext.Id));

            if (!deleteResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"failed HTTP request: {deleteResponse.StatusCode}\n{deleteResponse.ReasonPhrase}");
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