using CloudinaryDotNet;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PacientIK.Application.DTOs;
using PacientIK.Application.SavePhotoFunc;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration cfg)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                cfg.NotificationPublisher = new TaskWhenAllPublisher();
            });
            services.AddScoped<SavePhoto>();
            services.AddSingleton<SavePhotoId>();
            services.AddSingleton(new Cloudinary(cfg["Cloudinary:URL"]));
            return services;
        }
    }
}
