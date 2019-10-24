using System;
using Bog.Cmd.Domain.Models;
using System.Threading.Tasks;

namespace Bog.Cmd.CommandLine.Hosting
{
    public class BogApplicationRunner : IBogApplicationRunner
    {
        private readonly BogApplicationBuilderFactory _builderFactory;
        private readonly CommandArgs _args;

        public BogApplicationRunner(BogApplicationBuilderFactory builderFactory, CommandArgs args)
        {
            _builderFactory = builderFactory;
            _args = args;
        }

        public async Task RunAsync()
        {
            //TODO need to surround this with better exception handling when done
            var app = _builderFactory.Build();
            app.Execute(_args.Args);

            //Environment.Exit(0);

            await Task.CompletedTask;
        }
    }
}