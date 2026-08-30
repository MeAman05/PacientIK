using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Security.Claims;
using System.Text;

namespace PacientIK.Jwt
{
    public static class AuthExtension
    {
        public static IServiceCollection AddAuthExtension(this IServiceCollection services, IConfiguration cfg)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                
                .AddJwtBearer(j =>
                {
                    j.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = cfg["JwtConfig:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = cfg["JwtConfig:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["JwtConfig:Key"])),

                    };

                    j.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            ctx.Token = ctx.Request.Cookies["token"];
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
