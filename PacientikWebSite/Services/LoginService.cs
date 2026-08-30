using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using PacientikWebSite.Models;
using System.Net.Http.Json;

namespace PacientikWebSite.Services
{
    public class LoginService
    {
        private readonly HttpClient http;
        private readonly IJSRuntime js;
        private readonly NavigationManager nav;
        private readonly AuthState state;
        public LoginService(IHttpClientFactory htttp, IJSRuntime jss, NavigationManager naf, AuthState state)
        {
            http = htttp.CreateClient("Client");
            js = jss;
            nav = naf;
            this.state = state;
        }
        public async Task Login(LoginModel model)
        {
            
            var loingreq = new HttpRequestMessage(HttpMethod.Post, "/api/login/login");
            loingreq.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            loingreq.Content = JsonContent.Create(model);
            var loginResponse = await http.SendAsync(loingreq);
            if (!loginResponse.IsSuccessStatusCode)
            {
                return;
            }

            var result = await loginResponse.Content.ReadFromJsonAsync<TokenModel>();
            if (result != null)
            {
                switch (result.Role)
                {
                    case "Admin":
                        nav.NavigateTo("/adminpage");
                    break;
                    case "User":
                        nav.NavigateTo("/userpage");
                    break;
                    case "Checker":
                        nav.NavigateTo("/checkpage");
                    break;
                }
            }
        }

        public async Task Logout()
        {
            var Txt = new
            {
                txt = ""
            };
            
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/login/logout")
            {
                Content = JsonContent.Create(Txt)
            };
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            var response = await http.SendAsync(request);
            state.Logout();

        }

       
    }
}
