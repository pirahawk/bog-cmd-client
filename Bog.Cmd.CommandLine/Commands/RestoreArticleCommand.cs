using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.FileIO;
using Bog.Cmd.Domain.Values;
using System;
using System.Threading.Tasks;

namespace Bog.Cmd.CommandLine.Commands
{
    public class RestoreArticleCommand: IRestoreArticleCommand
    {
        private readonly IClientFileProvider _fileProvider;
        private readonly IUpdateArticleContextWorkflow _updateArticleContextWorkflow;

        public RestoreArticleCommand(IClientFileProvider fileProvider, IUpdateArticleContextWorkflow updateArticleContextWorkflow)
        {
            _fileProvider = fileProvider;
            _updateArticleContextWorkflow = updateArticleContextWorkflow;
        }

        public async Task Restore(string articleIdValue)
        {
            if (articleIdValue == null) throw new ArgumentNullException(nameof(articleIdValue));

            if (_fileProvider.CheckMetaFileExists(MetaFileNameValues.ARTICLE))
            {
                Console.WriteLine("An Article already exists within the current context");
                return;
            }

            var articleId = Guid.Parse(articleIdValue);
            await _updateArticleContextWorkflow.GetAndUpdateArticleContext(articleId);
        }
    }
}