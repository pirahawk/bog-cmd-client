using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Values;
using System;

namespace Bog.Cmd.CommandLine.Builders
{
    public class CreateCommandBuilder : IApplicationBuilder
    {
        private readonly ICreateArticleCommand _createArticleCommand;

        public CreateCommandBuilder(ICreateArticleCommand createArticleCommand)
        {
            _createArticleCommand = createArticleCommand;
        }

        public void Build(BogApiClientApplication clientApplication)
        {
            if (clientApplication == null) throw new ArgumentNullException(nameof(clientApplication));

            clientApplication.Command(CommandTypeNameValues.CREATE_COMMAND, (app) =>
                {
                    app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);
                    app.Description = "Create Command";

                    app.OnExecute(() =>
                    {
                        app.ShowHint();
                        return 0;
                    });
                });
        }
    }
}