using System;
using System.Threading;
using System.Threading.Tasks;
using Bog.Cmd.Domain.Application;
using Bog.Cmd.Domain.Models;
using Microsoft.Extensions.Hosting;

namespace Bog.Cmd.CommandLine.Hosting
{
    public class BogApiClientService : IHostedService, IDisposable
    {
        private readonly IBogApiClientApplication _app;
        private readonly CommandArgs _args;
        private readonly IApplicationLifetime _lifeTime;

        public BogApiClientService(IBogApiClientApplication app, CommandArgs args, IApplicationLifetime lifeTime)
        {
            _app = app;
            _args = args;
            _lifeTime = lifeTime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var result = _app.Execute(_args.Args);
           
            return Task.FromResult(1);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _lifeTime.StopApplication();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}