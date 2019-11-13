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
    public class UpdateArticleContextWorkflow : IUpdateArticleContextWorkflow
    {
        private readonly BogHttpClient _client;
        private readonly IClientFileProvider _fileProvider;

        public UpdateArticleContextWorkflow(BogHttpClient client, IClientFileProvider fileProvider)
        {
            _client = client;
            _fileProvider = fileProvider;
        }

        public async Task GetAndUpdateArticleContext(ArticleResponse articleContext)
        {
            if (articleContext == null) throw new ArgumentNullException(nameof(articleContext));

            var getArticleResponse = await _client.GetMessage(BogApiRouteValues.ARTICLE_GUID_FORMAT(articleContext.Id));

            if (!getArticleResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"failed HTTP request: {getArticleResponse.StatusCode}\n{getArticleResponse.ReasonPhrase}");
            }

            var blogContentTask = getArticleResponse.Content.ReadAsStringAsync();
            await _fileProvider.WriteMetaFile(MetaFileNameValues.ARTICLE, blogContentTask);
            var contents = await blogContentTask;
            contents = JsonUtility.Prettify<ArticleResponse>(contents);
            Console.WriteLine(contents);
        }
    }
}