using System;
using System.Threading.Tasks;
using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Builders
{
    public class PingCommandBuilder : IApplicationBuilder
    {
        private readonly IPingCommand _pingCommand;

        public PingCommandBuilder(IPingCommand pingCommand)
        {
            _pingCommand = pingCommand;
        }

        public void Build(BogApiClientApplication clientApplication)
        {
            if (clientApplication == null) throw new ArgumentNullException(nameof(clientApplication));

            clientApplication.Command(CommandTypeNameValues.PING_COMMAND, (app) =>
            {
                app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);
                app.Description = "Ping test for API health-check";

                app.OnExecute(async () =>
                {
                    await _pingCommand.SendHealthCheckPing();
                    return 0;
                });
            });
            
        }
    }
}