using System.Threading.Tasks;

namespace Bog.Cmd.Domain.Commands
{
    public interface IUpdateEntryCommand
    {
        Task UpdateEntry(string entryFilePath);
    }
}