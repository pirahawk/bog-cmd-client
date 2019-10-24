using System;
using System.IO;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.Domain.FileIO
{
    public class ClientFileProvider : IClientFileProvider
    {
        public DirectoryInfo GetCurrentDirectory()
        {
            var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
            return currentDirectory;
        }

        public bool CheckMetaFileExists(string metaFileName)
        {
            var currentExecutionPath = GetCurrentDirectory().FullName;
            var pathToFile = $"{currentExecutionPath}{Path.DirectorySeparatorChar}{MetaFileNameValues.META_FOLDER}{Path.DirectorySeparatorChar}{metaFileName}";
            var pathTestResult = File.Exists(pathToFile);
 
            return pathTestResult;
        }
    }
}