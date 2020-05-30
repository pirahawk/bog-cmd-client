using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.CommandLine.Extensions;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Builders
{
    public class UpdateEntryCommandBuilder : IApplicationBuilder
    {
        private readonly IUpdateEntryCommand _updateEntryCommand;

        public UpdateEntryCommandBuilder(IUpdateEntryCommand updateEntryCommand)
        {
            _updateEntryCommand = updateEntryCommand;
        }

        public void Build(BogApiClientApplication clientApplication)
        {
            var updateCommand = clientApplication.FindCommandByName(CommandTypeNameValues.UPDATE_COMMAND);

            updateCommand.Command(CreateSubCommands.ENTRY, app =>
            {
                app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);
                app.Description = "update a bog article entry within the current context";

                var entryFileName = app.Argument(CommandApplicationArgumentValues.FILE_NAME, CommandApplicationDescriptions.FILE_NAME);

                app.OnExecute(async () =>
                {
                    if (string.IsNullOrWhiteSpace(entryFileName.Value))
                    {
                        app.ShowHelp();
                        return 1;
                    }

                    await _updateEntryCommand.UpdateEntry(entryFileName.Value);
                    return 0;
                });
            });
        }
    }
}