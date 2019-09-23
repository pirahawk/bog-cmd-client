using Bog.Cmd.CommandLine;

namespace Bog.Cmd.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var build = ClientSetup.BuildClientHost();
            var hostTask = build.StartAsync();
            hostTask.Wait();
            build.StopAsync().Wait();
        }
    }
}
