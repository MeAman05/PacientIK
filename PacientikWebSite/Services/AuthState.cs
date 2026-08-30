using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using PacientikWebSite.Models;
using PacientikWebSite.Models.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;

namespace PacientikWebSite.Services
{
    public class AuthState : AuthenticationStateProvider
    {
        private readonly HttpClient http;
        private readonly UserStateModel model;
        private readonly NavigationManager nav;
        private readonly ClaimsPrincipal amogus = new(new ClaimsIdentity());
        private readonly IJSRuntime js;
        public AuthState(IHttpClientFactory http, UserStateModel model, NavigationManager nav, IJSRuntime js)
        {
            this.http = http.CreateClient("Client");
            this.model = model;
            this.nav = nav;
            this.js = js;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, "/api/login/logme");
                req.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

                var response = await http.SendAsync(req);
                if (!response.IsSuccessStatusCode)
                    return new AuthenticationState(amogus);

                var me = await response.Content.ReadFromJsonAsync<UserStateModel>();
                if (me == null) return new AuthenticationState(amogus);

                var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, me.uid),
            new(ClaimTypes.Role, me.role),
            new("specId", me.specId)
        };

                var identity = new ClaimsIdentity(claims, "cookie");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            catch
            {
                return new AuthenticationState(amogus);
            }
        }

        public void NotifyAuthChanged()
        {
            NotifyAuthenticationStateChanged(
                GetAuthenticationStateAsync()
            );
        }

        public void Logout()
        {

            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    new AuthenticationState(amogus)
                )
            );
        }
    }
}
