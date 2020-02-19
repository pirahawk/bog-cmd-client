using System.Threading.Tasks;
using Bog.Api.Domain.Models.Http;

namespace Bog.Cmd.CommandLine.Commands
{
    public interface IGetArticleContextWorkflow
    {
        Task<ArticleResponse> GetArticleContext();
    }
}