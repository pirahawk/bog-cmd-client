using Bog.Cmd.CommandLine.Application;
using Bog.Cmd.CommandLine.Extensions;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Values;
using System.Threading.Tasks;

namespace Bog.Cmd.CommandLine.Builders
{
    public class DeleteArticleCommandBuilder : IApplicationBuilder
    {
        private readonly IDeleteArticleCommand _deleteArticleCommand;

        public DeleteArticleCommandBuilder(IDeleteArticleCommand deleteArticleCommand)
        {
            _deleteArticleCommand = deleteArticleCommand;
        }

        public void Build(BogApiClientApplication clientApplication)
        {
            var deleteCommand = clientApplication.FindCommandByName(CommandTypeNameValues.DELETE_COMMAND);
           
            deleteCommand.Command(CreateSubCommands.ARTICLE, app =>
            {
                app.HelpOption(CommandApplicationConfigurationValues.HELP_TEMPLATE);
                app.Description = "mark bog article within the current context as deleted";

                app.OnExecute(async () =>
                {
                    await Task.CompletedTask;
                    await _deleteArticleCommand.MarkArticleAsDeleted();
                    return 0;
                });
            });
        }
    }
}