using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.CommandLine.Extensions;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Values;
using System.Linq;

namespace Bog.Cmd.CommandLine.Builders
{
    public class UpdateMetaTagsCommandBuilder : IApplicationBuilder
    {
        private IUpdateMetaTagsCommand _updateMetaTagsCommand;

        public UpdateMetaTagsCommandBuilder(IUpdateMetaTagsCommand updateMetaTagsCommand)
        {
            _updateMetaTagsCommand = updateMetaTagsCommand;
        }

        public void Build(BogApiClientApplication clientApplication)
        {
            var updateCommand = clientApplication.FindCommandByName(CommandTypeNameValues.UPDATE_COMMAND);

            updateCommand.Command(CreateSubCommands.META_TAG, app =>
            {
                app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);
                app.Description = "update meta tags on a bog article";

                var tags = app.Argument(CommandApplicationArgumentValues.TAG, CommandApplicationDescriptions.TAG, true);

                app.OnExecute(async () =>
                {
                    if (!tags.Values.Any())
                    {
                        app.ShowHelp();
                        return 0;
                    }

                    await _updateMetaTagsCommand.UpdateMetaTags(tags.Values.ToArray());
                    return 0;
                });
            });
        }
    }
}