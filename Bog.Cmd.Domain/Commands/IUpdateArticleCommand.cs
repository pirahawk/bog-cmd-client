using System.Threading.Tasks;

namespace Bog.Cmd.Domain.Commands
{
    public interface IUpdateArticleCommand
    {
        Task UpdateArticle(string author = null, string title = null, string description = null, bool? publish = null);
    }
}