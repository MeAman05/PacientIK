using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PacientIK.Domain.Entities;
using PacientIK.Domain.Repositories;
using PacientIK.Infrastructure.Context;
using PacientIK.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
        {

            services.AddDbContext<ApplicationDbContext>(db => db.UseNpgsql(cfg.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<ISpecReposotory, SpecRepository>();
            services.AddScoped<ILechRepository, LechRepository>();
            return services;
        }
    }
}
