using System;
using System.Threading;
using System.Threading.Tasks;
using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.Domain.Models;
using Microsoft.Extensions.Hosting;

namespace Bog.Cmd.CommandLine.Hosting
{
    //TODO: Delete when ready
    public class BogApiClientService : IHostedService, IDisposable
    {
        private readonly IBogApiClientApplication _app;
        private readonly CommandArgs _args;

        public BogApiClientService(IBogApiClientApplication app, CommandArgs args)
        {
            _app = app;
            _args = args;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var result = _app.Execute(_args.Args);
            return Task.FromResult(1);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}