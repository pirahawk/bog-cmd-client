using System.Threading.Tasks;

namespace Bog.Cmd.Domain.Commands
{
    public interface IUpdateArticleCommand
    {
        Task UpdateArticle(string author, bool publish);
    }
}