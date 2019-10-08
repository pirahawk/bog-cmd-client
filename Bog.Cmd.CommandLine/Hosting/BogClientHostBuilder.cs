using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
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
                AddJsonConfiguration(ctx, builder);
                AddCommandLineArguments(args, builder);
            };
        }

        private static void AddJsonConfiguration(HostBuilderContext ctx, IConfigurationBuilder builder)
        {
            var directoryContents = ctx.HostingEnvironment.ContentRootFileProvider
                .GetDirectoryContents("")
                .Where(fi => fi.Exists && !fi.IsDirectory && Path.GetExtension((string)fi.PhysicalPath).Contains("json"))
                .ToArray();

            foreach (IFileInfo fileInfo in directoryContents)
            {
                Console.WriteLine($"Path Found: {fileInfo.PhysicalPath}");
                builder.AddJsonFile(fileInfo.PhysicalPath);
            }
        }

        private static void AddCommandLineArguments(string[] args, IConfigurationBuilder builder)
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
        }

        private static void ConfigureHost(IConfigurationBuilder config)
        {
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<BlobApiSettings>((sp) =>
                context.Configuration.GetSection("BlobApiSettings").Get<BlobApiSettings>());

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