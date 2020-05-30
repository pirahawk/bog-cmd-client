using System;
using System.Linq;
using System.Threading.Tasks;
using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.CommandLine.Extensions;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Values;
using Microsoft.Extensions.CommandLineUtils;

namespace Bog.Cmd.CommandLine.Builders
{
    public class UpdateMediaCommandBuilder : IApplicationBuilder
    {
        private readonly IUpdateMediaCommand _updateMediaCommand;

        public UpdateMediaCommandBuilder(IUpdateMediaCommand updateMediaCommand)
        {
            _updateMediaCommand = updateMediaCommand;
        }

        public void Build(BogApiClientApplication clientApplication)
        {
            var updateCommand = clientApplication.FindCommandByName(CommandTypeNameValues.UPDATE_COMMAND);

            updateCommand.Command(CreateSubCommands.MEDIA, app =>
            {
                app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);
                app.Description = "Upload all bog article entry media within the current context";

                var mediaGlobPatternArgument = app.Argument(
                    CommandApplicationArgumentValues.MEDIA_GLOB_PATTERN, 
                    CommandApplicationDescriptions.MEDIA_GLOB_PATTERN,
                    (commandArg) => { commandArg.MultipleValues = true; });

                app.OnExecute(async () =>
                {
                    var mediaGlobPatterns = mediaGlobPatternArgument.Values;

                    if (mediaGlobPatternArgument.Values == null || !mediaGlobPatternArgument.Values.Any())
                    {
                        app.ShowHelp();
                        return 1;
                    }

                    await _updateMediaCommand.UpdateMediaFiles(mediaGlobPatterns.ToArray());
                    return 0;
                });
            });

        }
    }
}