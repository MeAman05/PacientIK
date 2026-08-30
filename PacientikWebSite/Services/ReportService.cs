using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using PacientikWebSite.Models.ReportModels;
using System.Diagnostics.Contracts;
using System.Net.Http.Json;

namespace PacientikWebSite.Services
{
    public class ReportService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager nav;

        public ReportService(IHttpClientFactory httpClient, NavigationManager nav)
        {
            _httpClient = httpClient.CreateClient("Client");
            this.nav = nav;
        }

        public async Task AddNewReport(CreateReportModel model)
        {
            var rquest = new HttpRequestMessage(HttpMethod.Post, "/api/doc/addreport")
            {
                Content = JsonContent.Create(model)
            };

            rquest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await _httpClient.SendAsync(rquest);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            else
            {
                nav.NavigateTo("/myreports");
            }
        }

        public async Task<List<ReportModel>> GetOnwReports(string? name)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/doc/getownreports?name={name}");
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await _httpClient.SendAsync(request);

            var model = await response.Content.ReadFromJsonAsync<List<ReportModel>>();

            return model;
        }


        public async Task <ReportModel> GetReportById(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/doc/getreportbyid/{id}");
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                nav.NavigateTo("/not-found");
            }

            var model = await response.Content.ReadFromJsonAsync<ReportModel>();

            return model;
        }


        public async Task ChangeReport(UpdateReportModel model, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/doc/update/{id}")
            {
                Content = JsonContent.Create(model)
            };
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await _httpClient.SendAsync(request)
            ;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }


        public async Task<List<ReportModel>> GetAllReport(string? name, string? snder, int lechid)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/doc/getallreports?name={name}&snder={snder}&lechid={lechid}");
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await _httpClient.SendAsync(request);

            var model = await response.Content.ReadFromJsonAsync<List<ReportModel>>();

            if(model != null)
            {
                return model;
            }
            else
            {
                return null;
            }
        }


        public async Task DeleteReport(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/doc/delete/{id}");
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}
