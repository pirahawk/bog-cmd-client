using System.Linq;
using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.Domain.Models;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Hosting
{
    public class BogApplicationBuilder
    {
        private readonly BogApiClientApplication _app;
        private readonly ICommandLineApplicationCommmandBuilder[] _builders;

        public BogApplicationBuilder(BogApiClientApplication app, params ICommandLineApplicationCommmandBuilder[] builders)
        {
            _app = app;
            _builders = builders;
        }

        public BogApiClientApplication Build()
        {
            _app.Name = CommandApplicationConfigurationValues.NAME;
            _app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);

            foreach (var builder in _builders ?? Enumerable.Empty<ICommandLineApplicationCommmandBuilder>().ToArray())
            {
                builder.Build(_app);
            }

            return _app;
        }
    }
}