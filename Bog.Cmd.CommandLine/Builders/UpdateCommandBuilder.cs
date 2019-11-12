using System;
using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Builders
{
    public class UpdateCommandBuilder : IApplicationBuilder
    {
        public void Build(BogApiClientApplication clientApplication)
        {
            if (clientApplication == null) throw new ArgumentNullException(nameof(clientApplication));

            clientApplication.Command(CommandTypeNameValues.UPDATE_COMMAND, (app) =>
            {
                app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);
                app.Description = "Update instances of bog entities stored in the current context";

                app.OnExecute(() =>
                {
                    app.ShowHint();
                    return 0;
                });
            });
        }
    }
}