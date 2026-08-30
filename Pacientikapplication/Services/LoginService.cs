using Pacientikapplication.Jwt;
using Pacientikapplication.Models.LoginSer;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Pacientikapplication.Services
{
    public class LoginService
    {
        private readonly HttpClient client;
        public string result { get; set; }
        public string page { get; set; }
        public LoginService(IHttpClientFactory http)
        {
            client = http.CreateClient("Client");
        }

        public async Task Login(LoginModel model)
        {
            var response = await client.PostAsJsonAsync("/api/login/login", model);

            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            var Result = await response.Content.ReadFromJsonAsync<JwtToken>();

            if(Result != null)
            {
                await SecureStorage.SetAsync("token", Result.Token);
                await SecureStorage.SetAsync("role", Result.Role);
                await SecureStorage.SetAsync("uid", Result.UserId.ToString());
                await SecureStorage.SetAsync("spec", Result.Spec.ToString());
                await SecureStorage.SetAsync("date", Result.Date.ToString());
                result = Result.Role;
            }

            switch (result)
            {
                case "Admin":
                    page = "/adminpage";
                break;
                case "User":
                    page = "/userpage";
                    break;
                case "Checker":
                    page = "/checkerpage";
                    break;

                default:
                    page = "/loginn";
                    break;
            }
        }

        public async Task<string> CheckToken()
        {
            var token = await SecureStorage.GetAsync("token");

            if(token == null)
            {
                return null;
            }

            return "Ok";
        }

        public async Task CheckRole(string role)
        {
            var Role = await SecureStorage.GetAsync("role");

            role = Role.ToString();
        }
    }
}
