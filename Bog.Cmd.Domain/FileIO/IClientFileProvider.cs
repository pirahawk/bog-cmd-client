using System.IO;

namespace Bog.Cmd.Domain.FileIO
{
    public interface IClientFileProvider
    {
        DirectoryInfo GetCurrentDirectory();
        bool CheckMetaFileExists(string article);
    }
}