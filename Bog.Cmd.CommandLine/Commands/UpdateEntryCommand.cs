using Bog.Cmd.CommandLine.Http;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.FileIO;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Values;
using Bog.Cmd.Common.Json;
using Bog.Cmd.Domain.Extensions;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Commands
{
    public class UpdateEntryCommand : IUpdateEntryCommand
    {
        private readonly IClientFileProvider _fileProvider;
        private readonly BogHttpClient _client;

        public UpdateEntryCommand(IClientFileProvider fileProvider, BogHttpClient client)
        {
            _fileProvider = fileProvider;
            _client = client;
        }

        public async Task UpdateEntry(string entryFilePath)
        {

            entryFilePath = Path.GetFullPath(entryFilePath);
            Console.WriteLine($"attempting to load entry file: {entryFilePath}");

            if (!File.Exists(entryFilePath))
            {
                throw new FileNotFoundException($"Could not load entry file: {entryFilePath}");
            }

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

            var entryFileBytes = await File.ReadAllBytesAsync(entryFilePath);
            var entryContentsUtf8 = Encoding.UTF8.GetString(entryFileBytes);

            var entry = new ArticleEntry
            {
                Content = entryContentsUtf8
            };

            var updateEntryResponse = await _client.PostMessage(articleContext.GetApiLink(LinkRelValueObject.ENTRY), entry);

            if (!updateEntryResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"failed HTTP request: {updateEntryResponse.StatusCode}\n{updateEntryResponse.ReasonPhrase}");
            }

            var entryResponseContents = await updateEntryResponse.Content.ReadAsStringAsync();
            entryResponseContents = JsonUtility.Prettify<ArticleEntryResponse>(entryResponseContents);

            Console.WriteLine(entryResponseContents);

            await Task.CompletedTask;
        }
    }
}