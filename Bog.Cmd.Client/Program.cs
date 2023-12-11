using Bog.Cmd.CommandLine.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

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
                var cmdAppRunner = scope.ServiceProvider.GetService<IBogApplicationRunner>();

                if (cmdAppRunner == null)
                {
                    return;
                }

                Task.WaitAll(cmdAppRunner.RunAsync());
            }

            //var svc = ActivatorUtilities.CreateInstance<ImagingService>(host.Services);

            //try
            //{
            //    Task.WaitAll(host.StartAsync());
            //    Task.WaitAll(host.StopAsync());
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}

            //Environment.Exit(0);
        }
    }
    /*
     * Build me:
     *
     *  dotnet publish -r win-x64 --no-self-contained /p:PublishSingleFile=true
     *  dotnet publish -r win-x64 --no-self-contained /p:PublishSingleFile=true -o "C:\Code\blog\client-test"
     *
     */
}
