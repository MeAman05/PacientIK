using Microsoft.AspNetCore.Components.WebAssembly.Http;
using PacientikWebSite.Models.Lech;
using System.Net.Http.Json;

namespace PacientikWebSite.Services
{
    public class LechService
    {
        private readonly HttpClient client;
        public LechService(IHttpClientFactory client)
        {
            this.client = client.CreateClient("Client");
        }

        public async Task<List<LechModel>> GetLeches()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/lech/getallleches");
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode.ToString());
            }

            var model = await response.Content.ReadFromJsonAsync<List<LechModel>>();

            return model;
        }
    }
}
