using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bog.Api.Domain.Models.Http;
using Bog.Cmd.CommandLine.Http;
using Bog.Cmd.Common.Json;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Extensions;
using Bog.Cmd.Domain.FileIO;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Commands
{
    public class DeleteArticleCommand : IDeleteArticleCommand
    {
        private readonly BogHttpClient _client;
        private readonly IClientFileProvider _fileProvider;
        private readonly IUpdateArticleContextWorkflow _updateArticleContextWorkflow;

        public DeleteArticleCommand(BogHttpClient client,  IClientFileProvider fileProvider, IUpdateArticleContextWorkflow updateArticleContextWorkflow)
        {
            _client = client;
            _fileProvider = fileProvider;
            _updateArticleContextWorkflow = updateArticleContextWorkflow;
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

            var deleteResponse = await _client.DeleteMessage(articleContext.GetSelfApiLink());

            if (!deleteResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"failed HTTP request: {deleteResponse.StatusCode}\n{deleteResponse.ReasonPhrase}");
            }

            await _updateArticleContextWorkflow.GetAndUpdateArticleContext(articleContext);
        }
    }
}