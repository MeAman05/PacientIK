using Pacientikapplication.Models.UserSer;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Pacientikapplication.Services
{
    public class UserService
    {
        private HttpClient http;
        public UserService(IHttpClientFactory htpp)
        {
            http = htpp.CreateClient("Client");
        }
        public async Task<UserModel> GetUserById(Guid id)
        {
            var response = await http.GetFromJsonAsync<UserModel>($"/api/user/getuserbyid/{id}");

            if(response == null)
            {
                return null;
            }

            return response;
        }
    }
}
