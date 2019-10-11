using System;
using System.Linq;
using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Builders
{
    public class CreateArticleCommandBuilder : IApplicationBuilder
    {
        public void Build(BogApiClientApplication clientApplication)
        {
            var createCommand = clientApplication.Commands.FirstOrDefault(c => c.Name == CommandTypeNameValues.CREATE_COMMAND);

            if (createCommand == null)
            {
                throw new Exception($"Could not find {CommandTypeNameValues.CREATE_COMMAND}");
            }

            createCommand.Command(CreateSubCommands.ARTICLE, app =>
            {
                app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);
                app.Description = "Nested Command";
                app.OnExecute(() =>
                {
                    return 0;
                });
            });
        }
    }
}