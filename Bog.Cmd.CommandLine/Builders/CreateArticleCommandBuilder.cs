using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.CommandLine.Extensions;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Builders
{
    public class CreateArticleCommandBuilder : IApplicationBuilder
    {
        private readonly ICreateArticleCommand _cmd;

        public CreateArticleCommandBuilder(ICreateArticleCommand cmd)
        {
            _cmd = cmd;
        }

        public void Build(BogApiClientApplication clientApplication)
        {
            var createCommand = clientApplication.FindCommandByName(CommandTypeNameValues.CREATE_COMMAND);

            createCommand.Command(CreateSubCommands.ARTICLE, app =>
            {
                app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);
                app.Description = "create a new bog article";

                var blogId = app.Argument(CommandApplicationArgumentValues.BLOG_ID, CommandApplicationArgumentDescriptions.BLOG_ID);
                var author = app.Argument(CommandApplicationArgumentValues.AUTHOR, CommandApplicationArgumentDescriptions.AUTHOR);


                app.OnExecute(async () =>
                {
                    if (string.IsNullOrWhiteSpace(blogId.Value) || string.IsNullOrWhiteSpace(author.Value))
                    {
                        app.ShowHelp();
                        return 1;
                    }

                    await _cmd.CreateArticle(blogId.Value, author.Value);
                    return 0;
                });
            });
        }
    }
}