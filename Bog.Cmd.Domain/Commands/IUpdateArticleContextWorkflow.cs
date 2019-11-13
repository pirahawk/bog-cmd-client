using System.Threading.Tasks;
using Bog.Api.Domain.Models.Http;

namespace Bog.Cmd.Domain.Commands
{
    public interface IUpdateArticleContextWorkflow
    {
        Task GetAndUpdateArticleContext(ArticleResponse articleContext);
    }
}