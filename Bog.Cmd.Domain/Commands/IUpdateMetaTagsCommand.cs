using System.Threading.Tasks;

namespace Bog.Cmd.Domain.Commands
{
    public interface IUpdateMetaTagsCommand
    {
        Task UpdateMetaTags(params string[] tags);
    }
}