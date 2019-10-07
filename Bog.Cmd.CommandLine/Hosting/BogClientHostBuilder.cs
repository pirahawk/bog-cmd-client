using System;
using System.Collections.Generic;
using System.Linq;
using Bog.Cmd.Domain.Application;
using Bog.Cmd.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bog.Cmd.CommandLine.Hosting
{
    public class BogClientHostBuilder
    {
        public static HostBuilder BuildClientHost(string[] args)
        {
            var builder = new HostBuilder();

            builder.ConfigureAppConfiguration(ConfigureApp(args));
            builder.ConfigureHostConfiguration(ConfigureHost);
            builder.ConfigureServices(ConfigureServices);

            return builder;
        }

        private static Action<HostBuilderContext, IConfigurationBuilder> ConfigureApp(string[] args)
        {
            return (ctx, builder) =>
            {
                if (args == null)
                {
                    builder.AddInMemoryCollection(new Dictionary<string, string>());
                    return;
                }

                var keyMappings = args.Select((val, index) => new
                {
                    Key = $"commandArgs:args:{index}",
                    Value = val
                }).ToDictionary(map => map.Key, map => map.Value);

                builder.AddInMemoryCollection(keyMappings);

            };
        }

        private static void ConfigureHost(IConfigurationBuilder config)
        {
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var commandArgs = context.Configuration.GetSection("commandArgs").Get<CommandArgs>();
            commandArgs ??= new CommandArgs{ Args = Enumerable.Empty<string>().ToArray()};
            services.AddSingleton(commandArgs);
            services.AddTransient<IBogApiClientApplication, BogApiClientApplication>();
            services.AddSingleton<IHostedService, BogApiClientService>();
        }
    }
}