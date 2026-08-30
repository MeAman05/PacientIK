using Pacientikapplication.Models;
using Pacientikapplication.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacientikapplication.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection Extensions(this IServiceCollection services)
        {
            
            services.AddHttpClient("Client", client =>
            {
                client.BaseAddress = new Uri("https://86b6-31-192-255-62.ngrok-free.app");
            });

            services.AddSingleton<LoginService>();
            services.AddSingleton<UserService>();
            return services;
        }
    }
}
