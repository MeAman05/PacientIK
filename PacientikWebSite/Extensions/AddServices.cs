using Microsoft.AspNetCore.Components.Authorization;
using PacientikWebSite.Models;
using PacientikWebSite.Models.ReportModels;
using PacientikWebSite.Services;
using PacientikWebSite.URIModel;

namespace PacientikWebSite.Extensions
{
    public static class AddServices
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
           
            services.AddHttpClient("Client", client =>
            {
                client.BaseAddress = new Uri("https://pacientik.onrender.com");
            });
            services.AddScoped<LoginService>();
            services.AddScoped<UserService>();
            services.AddScoped<SpecService>();
            services.AddScoped<AuthenticationStateProvider,AuthState>();
            services.AddScoped<AuthState>();
            services.AddScoped<UserStateModel>();
            services.AddScoped<LechService>();
            services.AddScoped<ReportService>();
            services.AddSingleton<ReportState>();
            
            return services;
        }
    }
}
