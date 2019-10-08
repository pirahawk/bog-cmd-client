using Bog.Cmd.CommandLine;
using Bog.Cmd.CommandLine.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

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
                var cmdAppRunner = scope.ServiceProvider.GetService(typeof(IBogApplicationRunner)) as IBogApplicationRunner;

                if (cmdAppRunner == null)
                {
                    return;
                }

                Task.WaitAll(cmdAppRunner.RunAsync());
            }
        }
    }
/*
 * Build me:
 *
 *  dotnet publish -r win-x64 --no-self-contained /p:PublishSingleFile=true  
 */
}
