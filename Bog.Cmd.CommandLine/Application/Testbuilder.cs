using System;

namespace Bog.Cmd.CommandLine.Application
{
    public class Testbuilder : ICommandLineApplicationCommmandBuilder
    {
        public void Build(BogApiClientApplication clientApplication)
        {
            if (clientApplication == null) throw new ArgumentNullException(nameof(clientApplication));


            clientApplication.Command("test1", (app) =>
            {

            });
        } 
    }
}