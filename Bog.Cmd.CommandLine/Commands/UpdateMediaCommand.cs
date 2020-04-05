using Bog.Api.Common;
using Bog.Api.Domain.Values;
using Bog.Cmd.CommandLine.Http;
using Bog.Cmd.Common.Glob;
using Bog.Cmd.Common.Http;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Extensions;
using Bog.Cmd.Domain.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Bog.Cmd.CommandLine.Commands
{
    public class UpdateMediaCommand : IUpdateMediaCommand
    {
        private IClientFileProvider _clientFileProvider;
        private readonly BogHttpClient _client;
        private readonly IGetLatestEntryContextForArticleWorkflow _getLatestEntryContextForArticleWorkflow;

        public UpdateMediaCommand(IClientFileProvider clientFileProvider, 
            BogHttpClient client, 
            IGetLatestEntryContextForArticleWorkflow getLatestEntryContextForArticleWorkflow)
        {
            _clientFileProvider = clientFileProvider;
            _client = client;
            _getLatestEntryContextForArticleWorkflow = getLatestEntryContextForArticleWorkflow;
        }

        public async Task UpdateMediaFiles(params string[] mediaGlobPatterns)
        {
            if (mediaGlobPatterns == null) throw new ArgumentNullException(nameof(mediaGlobPatterns));

            if (mediaGlobPatterns.Length <= 0)
            {
                return;
            }

            var latestArticleEntry = await _getLatestEntryContextForArticleWorkflow.GetLatestArticleEntryFromContext();

            if (latestArticleEntry == null)
            {
                Console.WriteLine("Could not get the latest Article entry for the article in the current context");
                return;
            }

            var latestArticleEntryMediaLink = latestArticleEntry.GetApiLink(LinkRelValueObject.MEDIA);
            var currentDirectory = _clientFileProvider.GetCurrentDirectory();
            var mediaFilesToUpload = FileGlobSearchUtility.FindAllFiles(currentDirectory.FullName, mediaGlobPatterns);
            
            if (mediaFilesToUpload == null || mediaFilesToUpload.Length <=0)
            {
                Console.WriteLine("No media files matched to upload!.");
                return;
            }

            Task.WaitAll(BeginFileUploads(mediaFilesToUpload, latestArticleEntryMediaLink).ToArray());

            await Task.CompletedTask;
        }

        public IEnumerable<Task> BeginFileUploads(string[] mediaFilesToUpload, string latestArticleEntryMediaLink)
        {
            foreach (var mediaFile in mediaFilesToUpload)
            {
                yield return BeginUpload(mediaFile, latestArticleEntryMediaLink);
            }
        }

        public async Task BeginUpload(string mediaFile, string latestArticleEntryMediaLink)
        {
            if (!File.Exists(mediaFile))
            {
                Console.WriteLine($"Upload Failed for '{mediaFile}': file does not exist!");
                return;
            }

            string contentTypeMapping;
            var couldMapType = new BogFileExtensionContentTypeProvider().TryGetContentType(mediaFile, out contentTypeMapping);

            if (!couldMapType)
            {
                Console.WriteLine($"Could not map a MIME type for file {mediaFile}");
                return;
            }

            var fileBytes = await File.ReadAllBytesAsync(mediaFile);
            var hash = fileBytes.ComputeMD5HashBase54();
            var contentDispositionHeaderValue = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Path.GetFileName(mediaFile),
            };

            var hasAlreadyBeenUploaded = await HasMediaAlreadyBeenUploadedForArticle(hash, contentDispositionHeaderValue, latestArticleEntryMediaLink);

            if (hasAlreadyBeenUploaded)
            {
                Console.WriteLine($"Skipping upload of media file: `{mediaFile}` to entryArticle: `{latestArticleEntryMediaLink}`");
                return;
            }
            
            var isFileUploaded = await UploadMediaFile(latestArticleEntryMediaLink, fileBytes, contentTypeMapping, contentDispositionHeaderValue);

            if (isFileUploaded)
            {
                Console.WriteLine($"Uploaded Media file: `{mediaFile}` to entryArticle: `{latestArticleEntryMediaLink}`");
                return;
            }

            Console.WriteLine($"Failed to upload media file: `{mediaFile}` to entryArticle: `{latestArticleEntryMediaLink}`");

        }

        private async Task<bool> UploadMediaFile(string articleEntryMediaLink,
            byte[] fileBytes,
            string contentType,
            ContentDispositionHeaderValue contentDispositionHeaderValue)
        {
            var response = await _client.PostRawMessage(articleEntryMediaLink, requestMessage =>
            {
                requestMessage.Content = new ByteArrayContent(fileBytes);
                requestMessage.Content.Headers.ContentDisposition = contentDispositionHeaderValue;
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            });

            return response.IsSuccessStatusCode;
        }

        private async Task<bool> HasMediaAlreadyBeenUploadedForArticle(string hash,
            ContentDispositionHeaderValue contentDispositionHeaderValue, 
            string latestArticleEntryMediaLink)
        {
            var response = await _client.HeadRawMessage(latestArticleEntryMediaLink, requestMessage =>
            {
                requestMessage.Headers.IfMatch.Add(new EntityTagHeaderValue($"\"{hash}\""));
                requestMessage.Content = new StringContent(string.Empty);
                requestMessage.Content.Headers.ContentDisposition = contentDispositionHeaderValue;
            });

            return response.IsSuccessStatusCode;
        }

        
    }
}