using Bog.Cmd.CommandLine.Application;

namespace Bog.Cmd.CommandLine.Builders
{
    public interface IApplicationBuilder
    {
        void Build(BogApiClientApplication clientApplication);
    }
}