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
    public class CreateArticleCommand : ICreateArticleCommand
    {
        private readonly BogHttpClient _client;
        private readonly IClientFileProvider _fileProvider;

        public CreateArticleCommand(BogHttpClient client, IClientFileProvider fileProvider)
        {
            _client = client;
            _fileProvider = fileProvider;
        }

        public async Task CreateArticle(string blogId, string author)
        {
            if (string.IsNullOrWhiteSpace(blogId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(blogId));
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(author));

            if (_fileProvider.CheckMetaFileExists(MetaFileNameValues.ARTICLE))
            {
                Console.WriteLine("An Article already exists within the current context");
                return;
            }

            var response = await _client.PostMessage(BogApiRouteValues.ARTICLE, new ArticleRequest
            {
                Author = author,
                BlogId = Guid.Parse(blogId)
            });

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"failed HTTP request: {response.StatusCode}\n{response.ReasonPhrase}");
            }

            var blogContentTask = response.Content.ReadAsStringAsync();
            await _fileProvider.WriteMetaFile(MetaFileNameValues.ARTICLE, blogContentTask);
            var contents = await blogContentTask;
            contents = JsonUtility.Prettify<ArticleResponse>(contents);
            Console.WriteLine(contents);
        }
    }
}