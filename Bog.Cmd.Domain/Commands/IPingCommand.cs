using System.Threading.Tasks;

namespace Bog.Cmd.Domain.Commands
{
    public interface IPingCommand
    {
        Task SendHealthCheckPing();
    }
}