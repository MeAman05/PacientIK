using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using PacientIK.Application.DTOs;
using PacientIK.Infrastructure.Context;
using PacientIK.Jwt;
using System.Security.Claims;

namespace PacientIK.Endpoitns
{
    public static class LoginEndpoint
    {
        public static void LoginMapEndoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/login", Login).AllowAnonymous();
            app.MapPost("/logout", Logout).AllowAnonymous();
            app.MapGet("/logme", LogMe).RequireAuthorization().CacheOutput(t => t.Expire(TimeSpan.FromMinutes(5)).Tag("clms"));
        }

        public static async Task<IResult> Login([FromBody] LoginDTO dTO, ApplicationDbContext context, JwtService service, HttpContext hcontext)
        {
            var currentuser = await context.Users.FirstOrDefaultAsync(u => u.Core == dTO.Code);

            if(currentuser == null)
            {
                return Results.NotFound();
            }

            var result = BCrypt.Net.BCrypt.Verify(dTO.Password, currentuser.Password);

            if (!result)
            {
                return Results.BadRequest();
            }

            var token = service.GenerateToken(currentuser);
            var session = new SessionModel
            {
                Token = token,
                UserId = currentuser.Id,
                Spec = currentuser.SpecId.ToString(),
                Role = currentuser.Role,
            };


            hcontext.Response.Cookies.Append("token", token,
                new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddHours(1),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                });

            return Results.Ok(session);
        }
        public static async Task<IResult> Logout(string? txt, HttpContext http, IOutputCacheStore cache)
        {
            http.Response.Cookies.Delete("token",new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.None
            });
            if(txt == null)
            {
                return Results.Ok();
            }

            await cache.EvictByTagAsync("clms", new CancellationToken());

            return Results.Ok();
        }

        public static async Task<IResult> LogMe(ClaimsPrincipal user)
        {
            var claims = new
            {
                uid = user.FindFirstValue(ClaimTypes.NameIdentifier),
                specId = user.FindFirstValue("specId"),
                role = user.FindFirstValue(ClaimTypes.Role)
            };

            return Results.Ok(claims);
        }
    }
}
