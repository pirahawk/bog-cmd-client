using System.Threading;
using System.Threading.Tasks;
using Bog.Cmd.CommandLine.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bog.Cmd.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientHostBuilder = BogClientHostBuilder.BuildClientHost(args);
            var host = clientHostBuilder.Build();

            using (var scope = host.Services.CreateScope())
            {
                var hostedService = scope.ServiceProvider.GetService(typeof(IHostedService)) as IHostedService;

                if (hostedService == null)
                {
                    return;
                }

                Task.WaitAll(hostedService.StartAsync(CancellationToken.None));
            }
        }
    }
}
