using System;
using System.Linq;
using Bog.Cmd.CommandLine.Application;
using Microsoft.Extensions.CommandLineUtils;

namespace Bog.Cmd.CommandLine.Extensions
{
    public static class BogApiClientApplicationExtensions
    {
        public static CommandLineApplication FindCommandByName(this BogApiClientApplication clientApplication, string commandName)
        {
            var command = clientApplication.Commands.FirstOrDefault(c => c.Name == commandName);

            if (command == null)
            {
                throw new Exception($"Could not find {commandName}");
            }

            return command;
        }
    }
}