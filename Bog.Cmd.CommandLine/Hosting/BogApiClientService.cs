using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bog.Cmd.CommandLine.Hosting
{
    //TODO: see https://andrewlock.net/introducing-ihostlifetime-and-untangling-the-generic-host-startup-interactions/
    public class BogApiClientService : IHostedService, IDisposable
    {
        private readonly IBogApplicationRunner _runner;

        public BogApiClientService(IBogApplicationRunner runner)
        {
            _runner = runner;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _runner.RunAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Environment.Exit(0);
            }
            
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Environment.Exit(0);
            await Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}