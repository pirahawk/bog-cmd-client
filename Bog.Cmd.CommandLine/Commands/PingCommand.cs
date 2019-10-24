using System;
using System.Threading.Tasks;
using Bog.Cmd.CommandLine.Http;
using Bog.Cmd.Domain.Commands;
using Bog.Cmd.Domain.Values;

namespace Bog.Cmd.CommandLine.Commands
{
    public class PingCommand : IPingCommand
    {
        private readonly BogHttpClient _client;

        public PingCommand(BogHttpClient client)
        {
            _client = client;
        }

        public async Task SendHealthCheckPing()
        {
            var response = await _client.GetMessage(BogApiRouteValues.PING);
            Console.WriteLine($"ping response: {response.StatusCode}\n{response}");
        }
    }
}