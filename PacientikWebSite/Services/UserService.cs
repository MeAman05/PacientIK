using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using PacientikWebSite.Models;
using PacientikWebSite.Models.UserModels;
using System.Net.Http.Json;
using System.Security.Claims;

namespace PacientikWebSite.Services
{
    public class UserService
    {
        private readonly HttpClient http;
        private readonly IJSRuntime js;
        private readonly NavigationManager nav;
        private readonly AuthState state;
        public UserService(IHttpClientFactory httpClient, IJSRuntime jes, NavigationManager naf, AuthState state)
        {
            http = httpClient.CreateClient("Client");
            js = jes;
            nav = naf;
            this.state = state;
        }

        public async Task<UserModel> GetUserById(Guid id)
        {
            
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"/api/user/getuserbyid/{id}");
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
                var response = await http.SendAsync(request);

                var result = await response.Content.ReadFromJsonAsync<UserModel>();

                return result;
            }
            catch(HttpRequestException ex) when(ex.StatusCode == System.Net.HttpStatusCode.Unauthorized) 
            {
                Console.WriteLine($"{ex.Message}");
                nav.NavigateTo("loginpage");
                return null;
            }
            catch
            {
                Console.WriteLine("404");
                return null;
            }
        }

        public async Task<List<UserModel>> GetAllUsers(string text)
        {
            var authState = await state.GetAuthenticationStateAsync();
            var user = authState.User;
            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            var myuid = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (role != "Admin")
            {
                nav.NavigateTo("/");
            }
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"/api/user/getusers?text={text}");
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
                var response = await http.SendAsync(request);

                var result = await response.Content.ReadFromJsonAsync<List<UserModel>>();
                result.RemoveAll(u => u.userid == Guid.Parse(myuid));
                return result;
            }
            catch(HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized) 
            {
                Console.WriteLine($"{ex.Message}");
                nav.NavigateTo("loginpage");
                return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                return null;
            }
        }

        public async Task AddNewUser(CreateUserModel model)
        {

            var content = new MultipartFormDataContent
            {
                { new StringContent(model.name), "name" },
                { new StringContent(model.surname), "surname" },
                { new StringContent(model.lastname), "lastname" },
                { new StringContent(model.code), "code" },
                { new StringContent(model.pwd), "pwd" },
                { new StringContent(model.spec.ToString()), "spec" },
                { new StringContent(model.age.ToString()), "age" },
                { new StringContent(model.role), "role" },
                { new StreamContent(model.photo.OpenReadStream()), "photo", model.photo.Name }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/user/create")
            {
                Content = content
            };
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            var response = await http.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                nav.NavigateTo("/getallusers");
            }
        }

        public async Task UpdateUser(UpdateUserModel model, Guid id)
        {

            var content = new MultipartFormDataContent();

            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                content.Add(new StringContent(model.Name), "Name");
            }
                
            if (!string.IsNullOrWhiteSpace(model.Surname))
            {
                content.Add(new StringContent(model.Surname), "Surname");
            }
            if (!string.IsNullOrWhiteSpace(model.Lastname))
            {
                content.Add(new StringContent(model.Lastname), "Lastname");
            }

            content.Add(new StringContent(model.Spec.ToString()), "Spec");
            content.Add(new StringContent(model.Age.ToString()), "Age");

            if (!string.IsNullOrWhiteSpace(model.Role))
                content.Add(new StringContent(model.Role), "Role");

            if (model.Photo != null)
            {
                content.Add(new StreamContent(model.Photo.OpenReadStream()), "Photo", model.Photo.Name);
            }

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/user/update/{id}")
            {
                Content = content
            };
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            var response = await http.SendAsync(request);
            response.EnsureSuccessStatusCode();
            nav.NavigateTo("/getallusers");
        }

        public async Task DeleteUser(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/user/delete/{id}");
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}
