using System;
using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Builders
{
    public class DeleteCommandBuilder : IApplicationBuilder
    {
        public void Build(BogApiClientApplication clientApplication)
        {
            if (clientApplication == null) throw new ArgumentNullException(nameof(clientApplication));

            clientApplication.Command(CommandTypeNameValues.DELETE_COMMAND, (app) =>
            {
                app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);
                app.Description = "Mark instances of bog entities stored in the current context as deleted";

                app.OnExecute(() =>
                {
                    app.ShowHint();
                    return 0;
                });
            });
        }
    }
}