using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using PacientikWebSite.Models.SpecModels;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace PacientikWebSite.Services
{
    public class SpecService
    {
        private readonly HttpClient http;
        private readonly IJSRuntime js;
        private readonly NavigationManager nav;
        private readonly AuthState state;
        public SpecService(IHttpClientFactory http, IJSRuntime js, NavigationManager naf, AuthState state)
        {
            this.http = http.CreateClient("Client");
            this.js = js;
            nav = naf;
            this.state = state;
        }

        public async Task<List<SpecModel>> GetSpeces()
        {
            var authState = await state.GetAuthenticationStateAsync();
            var user = authState.User;

            var role = user.FindFirst(ClaimTypes.Role)?.Value;

            if (role != "Admin")
            {
                nav.NavigateTo("/");
            }
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/api/spec/getallspeces");
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
                var response = await http.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("401 - кука не дошла");
                    nav.NavigateTo("/loginpage");
                    return new List<SpecModel>();
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    Console.WriteLine("403 - кука дошла, но роль не подошла. Проблема в Claims");
                    return new List<SpecModel>();
                }

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<List<SpecModel>>();

                return result;
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"{ex.Message}");
                return new List<SpecModel>();
            }
        }
    }
}
