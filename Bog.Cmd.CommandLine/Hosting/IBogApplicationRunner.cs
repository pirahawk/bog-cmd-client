using System.Threading.Tasks;

namespace Bog.Cmd.CommandLine.Hosting
{
    public interface IBogApplicationRunner
    {
        Task RunAsync();
    }
}