using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.CommandLine.Extensions;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Values;
using System;

namespace Bog.Cmd.CommandLine.Builders
{
    public class RestoreArticleCommandBuilder : IApplicationBuilder
    {
        private readonly IRestoreArticleCommand _restoreArticleCommand;

        public RestoreArticleCommandBuilder(IRestoreArticleCommand restoreArticleCommand)
        {
            _restoreArticleCommand = restoreArticleCommand;
        }
        public void Build(BogApiClientApplication clientApplication)
        {
            var restoreCommand = clientApplication.FindCommandByName(CommandTypeNameValues.RESTORE_COMMAND);

            if (restoreCommand == null)
            {
                throw new Exception($"Could not find {CommandTypeNameValues.RESTORE_COMMAND}");
            }

            restoreCommand.Command(CreateSubCommands.ARTICLE, app =>
            {
                app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);
                app.Description = "restore an existing bog article";

                var articleId = app.Argument(CommandApplicationArgumentValues.ARTICLE_ID, CommandApplicationArgumentDescriptions.ARTICLE_ID);

                app.OnExecute(async () =>
                {
                    if (string.IsNullOrWhiteSpace(articleId.Value) || string.IsNullOrWhiteSpace(articleId.Value))
                    {
                        app.ShowHelp();
                        return 1;
                    }

                    await _restoreArticleCommand.Restore(articleId.Value);
                    return 0;
                });
            });

        }
    }
}