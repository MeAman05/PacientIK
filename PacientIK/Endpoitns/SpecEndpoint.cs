using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using PacientIK.Application.Commands.Specs;
using PacientIK.Application.DTOs;
using PacientIK.Application.Queries.Specs;

namespace PacientIK.Endpoitns
{
    public static class SpecEndpoint
    {
        public static void SpecMapEndoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/getallspeces", GetAllSpeces).RequireAuthorization().CacheOutput(t => t.Expire(TimeSpan.FromMinutes(5)).Tag("speces"));
            app.MapGet("/getspecbyid/{id:int}", GetSpecById).RequireAuthorization(r => r.RequireRole("Admin")).CacheOutput(t => t.Expire(TimeSpan.FromMinutes(5)).Tag("specid")); ;
            app.MapPost("/addspec", AddNewSpec).RequireAuthorization(r => r.RequireRole("Admin"));
            app.MapPut("/updatespec/{id:int}", UpdateSpec).RequireAuthorization(r => r.RequireRole("Admin"));
            app.MapDelete("/deletespec/{id:int}", DeleteSpec).RequireAuthorization(r => r.RequireRole("Admin"));
        }

        public static async Task<IResult> GetAllSpeces(ISender sender)
        {
            var result = await sender.Send(new GetAllSpecQuery());

            return Results.Ok(result);
        }

        public static async Task<IResult> GetSpecById(ISender sender, int id)
        {
            var result = await sender.Send(new GetSpecById(id));

            return Results.Ok(result);
        }

        public static async Task<IResult> AddNewSpec(ISender sender, CreateSpecDTO dto, IOutputCacheStore cache)
        {
            var addspec = await sender.Send(new AddSepcCommand(dto));
            await cache.EvictByTagAsync("speces", new CancellationToken());
            await cache.EvictByTagAsync("specid", new CancellationToken());
            return Results.Ok(addspec);
        }

        public static async Task<IResult> UpdateSpec(ISender sender, UpdateSpecDTO dto, int id, IOutputCacheStore cache)
        {
            var updatespec = await sender.Send(new UpdateSpecCommand(dto, id));
            await cache.EvictByTagAsync("speces", new CancellationToken());
            await cache.EvictByTagAsync("specid", new CancellationToken());
            return Results.Ok(updatespec);
        }

        public static async Task<IResult> DeleteSpec(ISender sender, int id, IOutputCacheStore cache)
        {
            await sender.Send(new DeleteSpecCommand(id));
            await cache.EvictByTagAsync("speces", new CancellationToken());
            await cache.EvictByTagAsync("specid", new CancellationToken());
            return Results.Ok("deleted");
        }
    }
}
