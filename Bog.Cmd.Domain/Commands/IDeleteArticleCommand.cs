using System.Threading.Tasks;

namespace Bog.Cmd.Domain.Commands
{
    public interface IDeleteArticleCommand
    {
        Task MarkArticleAsDeleted();
    }
}