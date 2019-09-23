using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bog.Cmd.CommandLine
{
    public class ClientSetup
    {
        public static IHost BuildClientHost()
        {
            var builder = new HostBuilder();
            builder.ConfigureServices(ConfigureServices);
            builder.ConfigureHostConfiguration(ConfigureHost);
            var host = builder.Build();
            return host;
        }

        private static void ConfigureHost(IConfigurationBuilder config)
        {
            
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IBogApiClientApplication, BogApiClientApplication>();
            serviceCollection.AddSingleton<IHostedService, BogApiClientService>();
        }
    }


    public interface IBogApiClientApplication
    {
        int Execute(params string[] args);
    }

    public class BogApiClientApplication : CommandLineApplication, IBogApiClientApplication
    {

    }

    public class BogApiClientService : IHostedService, IDisposable
    {
        private readonly IBogApiClientApplication _app;

        public BogApiClientService(IBogApiClientApplication app)
        {
            _app = app;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //var result = _app.Execute();
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