using System.Threading.Tasks;

namespace Bog.Cmd.CommandLine
{
    public interface IBogApplicationRunner
    {
        Task RunAsync();
    }
}