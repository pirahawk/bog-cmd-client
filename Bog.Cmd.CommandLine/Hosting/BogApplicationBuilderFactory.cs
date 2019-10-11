using System.Linq;
using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.CommandLine.Builders;
using Bog.Cmd.CommandLine.Commands;
using Bog.Cmd.Domain.Models;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Hosting
{
    public class BogApplicationBuilderFactory
    {
        private readonly BogApiClientApplication _app;
        private readonly IApplicationBuilder[] _builders;

        public BogApplicationBuilderFactory(BogApiClientApplication app, params IApplicationBuilder[] builders)
        {
            _app = app;
            _builders = builders;
        }

        public BogApiClientApplication Build()
        {
            foreach (var builder in _builders ?? Enumerable.Empty<IApplicationBuilder>().ToArray())
            {
                builder.Build(_app);
            }

            return _app;
        }
    }
}