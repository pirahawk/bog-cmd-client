﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.CommandLine.Builders;
using Bog.Cmd.CommandLine.Commands;
using Bog.Cmd.CommandLine.Http;
using Bog.Cmd.Domain.Commands;
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
                AddCommandLineArgumentsConfiguration(args, builder);
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
                builder.AddJsonFile(fileInfo.PhysicalPath);
            }
        }

        private static void AddCommandLineArgumentsConfiguration(string[] args, IConfigurationBuilder builder)
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

            services.AddTransient<BogApplicationBuilderFactory>((sp) =>
            {
                var bogApiClientApplication = sp.GetService<BogApiClientApplication>();
                var builders = sp.GetServices<IApplicationBuilder>()
                    .DefaultIfEmpty()
                    .ToArray();

                return new BogApplicationBuilderFactory(bogApiClientApplication, builders);
            });

            services.AddTransient<BogHttpClient>((sp) =>
            {
                var apiSettings = sp.GetService<BlobApiSettings>();
                var clientBaseAddress = new Uri($"{apiSettings.Scheme}://{apiSettings.Host}");
                var client = new BogHttpClient();
                client.BaseAddress = clientBaseAddress;
                return client;
            });

            services.AddTransient<BogApiClientApplication>();
            services.AddTransient<IBogApplicationRunner, BogApplicationRunner>();
            services.AddSingleton<IHostedService, BogApiClientService>();

            services.AddTransient<IApplicationBuilder, RootApplicationBuilder>();
            services.AddTransient<IApplicationBuilder, CreateCommandBuilder>();
            services.AddTransient<IApplicationBuilder, CreateArticleCommandBuilder>();

            services.AddTransient<ICreateArticleCommand, CreateArticleCommand>();
        }
    }
}