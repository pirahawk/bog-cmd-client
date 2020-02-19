using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.CommandLine.Extensions;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Values;
using Microsoft.Extensions.CommandLineUtils;

namespace Bog.Cmd.CommandLine.Builders
{
    public class UpdateArticleCommandBuilder : IApplicationBuilder
    {
        private readonly IUpdateArticleCommand _updateArticleCommand;

        public UpdateArticleCommandBuilder(IUpdateArticleCommand updateArticleCommand)
        {
            _updateArticleCommand = updateArticleCommand;
        }

        public void Build(BogApiClientApplication clientApplication)
        {
            var updateCommand = clientApplication.FindCommandByName(CommandTypeNameValues.UPDATE_COMMAND);
            
            updateCommand.Command(CreateSubCommands.ARTICLE, app =>
            {
                app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);
                app.Description = "update a bog article within the current context";

                var author = app.Option(CommandApplicationOptionValues.AUTHOR, CommandApplicationArgumentDescriptions.AUTHOR, CommandOptionType.SingleValue);
                var publish = app.Option(CommandApplicationOptionValues.PUBLISH, CommandApplicationArgumentDescriptions.PUBLISH, CommandOptionType.NoValue);

                app.OnExecute(async () =>
                {
                    await _updateArticleCommand.UpdateArticle(author.Value(), publish.HasValue());
                    return 0;
                });
            });

        }
    }
}