using System;
using System.IO;
using System.Threading.Tasks;
using Bog.Cmd.Common.Json;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.Domain.FileIO
{
    public class ClientFileProvider : IClientFileProvider
    {
        public bool CheckMetaFileExists(string metaFileName)
        {
            var pathToFile = GetMetaFilePath(metaFileName);
            var pathTestResult = File.Exists(pathToFile);
 
            return pathTestResult;
        }

        public async Task WriteMetaFile(string metaFileName, Task<string> contentSourceProvider)
        {
            var content = await contentSourceProvider;
            var writePath = GetMetaFilePath(metaFileName);
            
            EnsureRootDirectoryExists();

            using (var fileStream = File.Create(writePath))
            {
                using (var sw = new StreamWriter(fileStream))
                {
                    await sw.WriteAsync(content.ToCharArray(), 0, content.Length);
                }
            }
        }

        public async Task<TModel> ReadMetaFile<TModel>(string metaFileName)
        {
            if (!CheckMetaFileExists(metaFileName))
            {
                return default(TModel);
            }

            var readPath = GetMetaFilePath(metaFileName);
            var readAllTextAsync = await File.ReadAllTextAsync(readPath);
            var model = JsonUtility.Deserialize<TModel>(readAllTextAsync);
            return model;
        }

        private void EnsureRootDirectoryExists()
        {
            var metaFolderRootPath = GetMetaFolderRootPath();
            if (Directory.Exists(metaFolderRootPath))
            {
                return;
            }

            Directory.CreateDirectory(metaFolderRootPath);
        }

        private string GetMetaFilePath(string metaFileName)
        {
            var metaFilePath = Path.Combine(GetMetaFolderRootPath(), metaFileName);
            return metaFilePath;
        }

        private string GetMetaFolderRootPath()
        {
            var currentExecutionPath = GetCurrentDirectory().FullName;
            var rootMetaPath = Path.Combine(currentExecutionPath, MetaFileNameValues.META_FOLDER);
            return rootMetaPath;
        }

        public DirectoryInfo GetCurrentDirectory()
        {
            var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
            return currentDirectory;
        }
    }
}