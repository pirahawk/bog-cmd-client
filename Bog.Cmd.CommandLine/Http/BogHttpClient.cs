using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Bog.Cmd.Common.Json;

namespace Bog.Cmd.CommandLine.Http
{
    public class BogHttpClient : HttpClient
    {
        public async Task<HttpResponseMessage> PostMessage<TRequest>(string apiRoute, TRequest requestBody)
        {
            return await SendRequestWithPayload(HttpMethod.Post, apiRoute, requestBody);
        }

        public async Task<HttpResponseMessage> PutMessage<TRequest>(string apiRoute, TRequest requestBody)
        {
            return await SendRequestWithPayload(HttpMethod.Put, apiRoute, requestBody);
        }

        public async Task<HttpResponseMessage> GetMessage(string apiRoute)
        {
            return await SendRequestNoPayload(apiRoute, HttpMethod.Get);
        }

        public async Task<HttpResponseMessage> DeleteMessage(string apiRoute)
        {
            return await SendRequestNoPayload(apiRoute, HttpMethod.Delete);
        }

        private async Task<HttpResponseMessage> SendRequestNoPayload(string apiRoute, HttpMethod httpMethod)
        {
            if (string.IsNullOrWhiteSpace(apiRoute))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiRoute));

            var request = new HttpRequestMessage(httpMethod, apiRoute);
            var response = await SendAsync(request);
            return response;
        }

        private async Task<HttpResponseMessage> SendRequestWithPayload<TRequest>(HttpMethod httpMethod, string apiRoute, TRequest requestBody)
        {
            if (requestBody == null) throw new ArgumentNullException(nameof(requestBody));
            if (string.IsNullOrWhiteSpace(apiRoute))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiRoute));

            var request = new HttpRequestMessage(httpMethod, apiRoute);
            var serializeObject = JsonUtility.Serialize(requestBody);
            request.Content = new StringContent(serializeObject);
            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json);

            var response = await SendAsync(request);

            return response;
        }
    }
}