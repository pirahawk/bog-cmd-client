using System.Threading.Tasks;

namespace Bog.Cmd.Domain.Commands
{
    public interface IRestoreArticleCommand
    {
        Task Restore(string articleIdValue);
    }
}