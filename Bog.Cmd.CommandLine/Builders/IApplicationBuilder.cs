using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Builders
{
    public interface IApplicationBuilder
    {
        void Build(BogApiClientApplication clientApplication);
    }

    public class RootApplicationBuilder : IApplicationBuilder
    {
        public void Build(BogApiClientApplication clientApplication)
        {
            clientApplication.Name = CommandApplicationConfigurationValues.NAME;
            clientApplication.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);

            clientApplication.OnExecute(() =>
            {
                clientApplication.ShowHint();
                return 0;
            });
        }
    }
}