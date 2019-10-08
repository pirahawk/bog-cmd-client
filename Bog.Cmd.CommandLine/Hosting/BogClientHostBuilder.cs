using System;
using System.Collections.Generic;
using System.Linq;
using Bog.Cmd.CommandLine.Application;
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
            services.AddSingleton<CommandArgs>((sp) =>
            {
                var commandArgs = context.Configuration.GetSection("commandArgs").Get<CommandArgs>();
                commandArgs ??= new CommandArgs { Args = Enumerable.Empty<string>().ToArray() };
                return commandArgs;
            });

            services.AddTransient<BogApplicationBuilder>((sp) =>
            {
                var bogApiClientApplication = sp.GetService(typeof(BogApiClientApplication)) as BogApiClientApplication;
                var builders = sp.GetServices<ICommandLineApplicationCommmandBuilder>()
                    .DefaultIfEmpty()
                    .ToArray();

                return new BogApplicationBuilder(bogApiClientApplication, builders);
            });

            services.AddTransient<BogApiClientApplication>();
            services.AddTransient<IBogApplicationRunner, BogApplicationRunner>();
            services.AddSingleton<IHostedService, BogApiClientService>();
            services.AddTransient<ICommandLineApplicationCommmandBuilder, Testbuilder>();
        }
    }
}