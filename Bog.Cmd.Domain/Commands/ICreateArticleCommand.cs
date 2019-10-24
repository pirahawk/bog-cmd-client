using System.Threading.Tasks;

namespace Bog.Cmd.Domain.Commands
{
    public interface ICreateArticleCommand
    {
        Task CreateArticle(string blogId, string author);
    }
}