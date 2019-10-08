using System;
using Microsoft.Extensions.CommandLineUtils;

namespace Bog.Cmd.CommandLine.Application
{
    public interface IBogApiClientApplication
    {
        int Execute(params string[] args);
        CommandLineApplication Command(string name, Action<CommandLineApplication> configurationAction, bool throwOnUnexpected = true);
    }
}