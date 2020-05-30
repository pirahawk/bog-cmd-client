using System.Threading.Tasks;

namespace Bog.Cmd.Domain.Commands
{
    public interface IDeleteMetaTagsCommand
    {
        Task DeleteMetaTags(params string[] tagsToDelete);
    }
}