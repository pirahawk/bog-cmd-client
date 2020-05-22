using System.Threading.Tasks;

namespace Bog.Cmd.Domain.Commands
{
    public interface ICreateArticleCommand
    {
        Task CreateArticle(string blogIdValue, string authorValue, string titleValue, string description = null);
    }
}