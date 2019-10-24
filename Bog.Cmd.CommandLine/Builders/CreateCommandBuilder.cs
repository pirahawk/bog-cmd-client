using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.Domain.Values;
using System;

namespace Bog.Cmd.CommandLine.Builders
{
    public class CreateCommandBuilder : IApplicationBuilder
    {
        public void Build(BogApiClientApplication clientApplication)
        {
            if (clientApplication == null) throw new ArgumentNullException(nameof(clientApplication));

            clientApplication.Command(CommandTypeNameValues.CREATE_COMMAND, (app) =>
                {
                    app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);
                    app.Description = "Create new instances of bog entities";

                    app.OnExecute(() =>
                    {
                        app.ShowHint();
                        return 0;
                    });
                });
        }
    }
}