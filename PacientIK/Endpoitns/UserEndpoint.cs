using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PacientIK.Application.Commands.Users;
using PacientIK.Application.DTOs;
using PacientIK.Application.Queries.Users;

namespace PacientIK.Endpoitns
{
    public static class UserEndpoint
    {
        public static void UserMapEndpoiint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/getusers", GetAllUsers).RequireAuthorization(r => r.RequireRole("Admin")).CacheOutput(t => t.Expire(TimeSpan.FromMinutes(5)).Tag("users"));
            app.MapGet("/getuserbyid/{id:guid}", GetUserById).RequireAuthorization(r => r.RequireRole("Admin","User", "Checker")).CacheOutput(t => t.Expire(TimeSpan.FromMinutes(5)).Tag("uid")); ;
            app.MapPost("/create", AddNewUser).DisableAntiforgery();
            app.MapPatch("/update/{id:guid}", UpdateUser).RequireAuthorization(r => r.RequireRole("Admin")).DisableAntiforgery();
            app.MapDelete("/delete/{id:guid}", DeleteUser).RequireAuthorization(r => r.RequireRole("Admin"));
        }

        public static async Task<IResult> GetAllUsers(ISender sender, string text)
        {
            var result = await sender.Send(new GetAllUsersQuery(text));

            return Results.Ok(result);
        }

        public static async Task<IResult> GetUserById(ISender sender, Guid id)
        {
            var result = await sender.Send(new GetUserByIdQuery(id));

            return Results.Ok(result);
        }

        public static async Task<IResult> AddNewUser(ISender sender, [FromForm] CreateUserDTO dTO, IOutputCacheStore cache)
        {
            var result = await sender.Send(new AddUserCommand(dTO));
            await cache.EvictByTagAsync("users", new CancellationToken());
            await cache.EvictByTagAsync("uid", new CancellationToken());
            return Results.Ok(result);
        }

        public static async Task<IResult> UpdateUser(ISender sender, [FromForm] UpdateUserDTO dTO, Guid id, IOutputCacheStore cache)
        {
            var result = await sender.Send(new UpdateUserCommand(dTO, id));
            await cache.EvictByTagAsync("users", new CancellationToken());
            await cache.EvictByTagAsync("uid", new CancellationToken());
            return Results.Ok(result);
        }

        public static async Task<IResult> DeleteUser(ISender sender, Guid id, IOutputCacheStore cache)
        {
            await sender.Send(new DeleteUserCommand(id));
            await cache.EvictByTagAsync("users", new CancellationToken());
            await cache.EvictByTagAsync("uid", new CancellationToken());
            return Results.Ok("Удалено");
        }
    }
}
