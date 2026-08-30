using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using PacientIK.Application.Commands.Lechs;
using PacientIK.Application.DTOs;
using PacientIK.Application.Queries.Lechs;

namespace PacientIK.Endpoitns
{
    public static class LechEndpoint
    {
        public static void LechMapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/getallleches", GetAllLeches).RequireAuthorization().CacheOutput(t => t.Expire(TimeSpan.FromMinutes(5)).Tag("les"));
            app.MapGet("/getlechbyid/{id:int}", GetLechById).RequireAuthorization(r => r.RequireRole("Admin")).CacheOutput(t => t.Expire(TimeSpan.FromMinutes(5)).Tag("lesid"));
            app.MapPost("/addlech", AddNewLech).RequireAuthorization(r => r.RequireRole("Admin"));
            app.MapPatch("/updatelech/{id:int}",UpdateLech).RequireAuthorization(r => r.RequireRole("Admin"));
            app.MapDelete("/deletelech/{id:int}", DeleteLech).RequireAuthorization(r => r.RequireRole("Admin"));
        }

        public static async Task<IResult> GetAllLeches(ISender sender)
        {
            var result = await sender.Send(new GetAllLechesQuery());

            return Results.Ok(result);
        }

        public static async Task<IResult> GetLechById(ISender sender, int id)
        {
            var result = await sender.Send(new GetLechByIdQuery(id));

            return Results.Ok(result);
        }

        public static async Task<IResult> AddNewLech(ISender sender, CreateLechDTO dto, IOutputCacheStore cache)
        {
            var result = await sender.Send(new AddNewLechCommand(dto));
            await cache.EvictByTagAsync("les", new CancellationToken());
            await cache.EvictByTagAsync("lesid", new CancellationToken());
            return Results.Ok(result);
        }

        public static async Task<IResult> UpdateLech(ISender sender, int id, UpdateLechDTO dTO, IOutputCacheStore cache)
        {
            var result = await sender.Send(new UpdateLechCommand(dTO, id));
            await cache.EvictByTagAsync("les", new CancellationToken());
            await cache.EvictByTagAsync("lesid", new CancellationToken());
            return Results.Ok(result);
        }

        public static async Task<IResult> DeleteLech(ISender sender, int id, IOutputCacheStore cache)
        {
            await sender.Send(new DeleteLechCommand(id));
            await cache.EvictByTagAsync("les", new CancellationToken());
            await cache.EvictByTagAsync("lesid", new CancellationToken());
            return Results.Ok("Deleted");
        }
    }
}
