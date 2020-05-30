using System.Linq;
using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.CommandLine.Extensions;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Builders
{
    public class DeleteMetaTagCommandBuilder : IApplicationBuilder
    {
        private readonly IDeleteMetaTagsCommand _deleteMetaTagsCommand;

        public DeleteMetaTagCommandBuilder(IDeleteMetaTagsCommand deleteMetaTagsCommand)
        {
            _deleteMetaTagsCommand = deleteMetaTagsCommand;
        }

        public void Build(BogApiClientApplication clientApplication)
        {
            var deleteCommand = clientApplication.FindCommandByName(CommandTypeNameValues.DELETE_COMMAND);
            deleteCommand.Command(CreateSubCommands.META_TAG, app =>
            {
                app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);
                app.Description = "delete meta tags on a bog article";

                var tags = app.Argument(CommandApplicationArgumentValues.TAG, CommandApplicationDescriptions.TAG, true);

                app.OnExecute(async () =>
                {
                    if (!tags.Values.Any())
                    {
                        app.ShowHelp();
                        return 0;
                    }

                    await _deleteMetaTagsCommand.DeleteMetaTags(tags.Values.ToArray());
                    return 0;
                });
            });
        }
    }
}