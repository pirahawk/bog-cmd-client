﻿using Bog.Cmd.CommandLine.Application;
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

                var author = app.Option(CommandApplicationOptionValues.AUTHOR, CommandApplicationDescriptions.AUTHOR, CommandOptionType.SingleValue);
                var title = app.Option(CommandApplicationOptionValues.TITLE, CommandApplicationDescriptions.TITLE, CommandOptionType.SingleValue);
                var description = app.Option(CommandApplicationOptionValues.DESCRIPTION, CommandApplicationDescriptions.DESCRIPTION, CommandOptionType.SingleValue);
                var publish = app.Option(CommandApplicationOptionValues.PUBLISH, CommandApplicationDescriptions.PUBLISH, CommandOptionType.NoValue);
                var unPublish = app.Option(CommandApplicationOptionValues.UNPUBLISH, CommandApplicationDescriptions.UNPUBLISH, CommandOptionType.NoValue);


                app.OnExecute(async () =>
                {
                    bool? isPublished = publish.HasValue()? true : null as bool?;
                    isPublished = unPublish.HasValue() ? false : isPublished;
                    await _updateArticleCommand.UpdateArticle(author.Value(), title.Value(), description.Value(), isPublished);
                    return 0;
                });
            });
        }
    }
}