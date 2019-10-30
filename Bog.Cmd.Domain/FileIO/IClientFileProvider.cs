using System.Threading.Tasks;

namespace Bog.Cmd.Domain.FileIO
{
    public interface IClientFileProvider
    {
        bool CheckMetaFileExists(string metaFileName);
        Task WriteMetaFile(string metaFileName, Task<string> contentSourceProvider);
    }
}