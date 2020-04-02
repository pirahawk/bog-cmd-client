using System.Threading.Tasks;

namespace Bog.Cmd.Domain.Commands
{
    public interface IUpdateMediaCommand
    {
        Task UpdateMediaFiles(params string[] mediaGlobPatterns);
    }
}