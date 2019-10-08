using Bog.Cmd.Domain.Models;
using System.Threading.Tasks;

namespace Bog.Cmd.CommandLine.Hosting
{
    public class BogApplicationRunner : IBogApplicationRunner
    {
        private readonly BogApplicationBuilder _builder;
        private readonly CommandArgs _args;

        public BogApplicationRunner(BogApplicationBuilder builder, CommandArgs args)
        {
            _builder = builder;
            _args = args;
        }

        public async Task RunAsync()
        {
            var app = _builder.Build();
            app.Execute(_args.Args);

            await Task.CompletedTask;
        }
    }
}