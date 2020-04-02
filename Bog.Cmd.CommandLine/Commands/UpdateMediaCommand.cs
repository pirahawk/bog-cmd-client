using System;
using System.Threading.Tasks;
using Bog.Cmd.Common.Glob;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.FileIO;

namespace Bog.Cmd.CommandLine.Commands
{
    public class UpdateMediaCommand : IUpdateMediaCommand
    {
        private IClientFileProvider _clientFileProvider;

        public UpdateMediaCommand(IClientFileProvider clientFileProvider)
        {
            this._clientFileProvider = clientFileProvider;
        }

        public async Task UpdateMediaFiles(params string[] mediaGlobPatterns)
        {
            if (mediaGlobPatterns == null) throw new ArgumentNullException(nameof(mediaGlobPatterns));

            if (mediaGlobPatterns.Length <= 0)
            {
                return;
            }

            var currentDirectory = _clientFileProvider.GetCurrentDirectory();
            var mediaFilesToUpload = FileGlobSearchUtility.FindAllFiles(currentDirectory.FullName, mediaGlobPatterns);

            await Task.CompletedTask;
        }
    }
}