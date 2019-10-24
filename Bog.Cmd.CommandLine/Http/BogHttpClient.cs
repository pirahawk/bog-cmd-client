using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bog.Cmd.CommandLine.Http
{
    public class BogHttpClient : HttpClient
    {
        public async Task<HttpResponseMessage> PostMessage<TRequest>(string apiRoute, TRequest requestBody)
        {
            if (requestBody == null) throw new ArgumentNullException(nameof(requestBody));
            if (string.IsNullOrWhiteSpace(apiRoute))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiRoute));

            var request = new HttpRequestMessage(HttpMethod.Post, apiRoute);
            //var serializeObject = JsonConvert.SerializeObject(requestBody);
            var serializeObject = JsonSerializer.Serialize(requestBody);
            request.Content = new StringContent(serializeObject);
            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json);

            var response = await SendAsync(request);

            return response;
        }

        public async Task<HttpResponseMessage> GetMessage(string apiRoute)
        {
            if (string.IsNullOrWhiteSpace(apiRoute))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiRoute));

            var request = new HttpRequestMessage(HttpMethod.Get, apiRoute);
            var response = await SendAsync(request);
            return response;
        }
    }
}