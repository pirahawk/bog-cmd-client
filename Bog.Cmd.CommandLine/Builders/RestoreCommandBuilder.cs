using System;
using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Builders
{
    public class RestoreCommandBuilder : IApplicationBuilder
    {
        public void Build(BogApiClientApplication clientApplication)
        {
            if (clientApplication == null) throw new ArgumentNullException(nameof(clientApplication));

            clientApplication.Command(CommandTypeNameValues.RESTORE_COMMAND, (app) =>
            {
                app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);
                app.Description = "Restore articles within the current directory";

                app.OnExecute(() =>
                {
                    app.ShowHint();
                    return 0;
                });
            });
        }
    }
}