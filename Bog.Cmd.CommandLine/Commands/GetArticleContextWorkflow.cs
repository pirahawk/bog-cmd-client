using System;
using System.Threading.Tasks;
using Bog.Api.Domain.Models.Http;
using Bog.Cmd.Domain.FileIO;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Commands
{
    public class GetArticleContextWorkflow : IGetArticleContextWorkflow
    {
        private readonly IClientFileProvider _fileProvider;

        public GetArticleContextWorkflow(IClientFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public async Task<ArticleResponse> GetArticleContext()
        {
            if (!_fileProvider.CheckMetaFileExists(MetaFileNameValues.ARTICLE))
            {
                Console.Error.WriteLine("No Article exists in current context");
                return null;
            }

            var articleContext = await _fileProvider.ReadMetaFile<ArticleResponse>(MetaFileNameValues.ARTICLE);
            return articleContext;
        }
    }
}