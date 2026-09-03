using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.JSInterop.Infrastructure;
using PacientIK.Application.Commands.Reports;
using PacientIK.Application.DTOs;
using PacientIK.Application.Queries.Reports;

namespace PacientIK.Endpoitns
{
    public static class ReportEndpoint
    {
        public static void ReportMapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/getallreports", GetAllReports).RequireAuthorization(r => r.RequireRole("Admin", "Checker")).CacheOutput(t => t.Expire(TimeSpan.FromMinutes(5)).Tag("reps"));
            app.MapGet("/getownreports", GetOwnReports).RequireAuthorization(r => r.RequireRole("Checker","Admin", "User")).CacheOutput(t => t.Expire(TimeSpan.FromMinutes(5)).Tag("ownreps"));
            app.MapGet("/getreps/{uid:guid}", GetReportsById).RequireAuthorization(r => r.RequireRole("Admin","Checker")).CacheOutput(t => t.Expire(TimeSpan.FromMinutes(5)).Tag("idreps"));
            app.MapGet("/getreportbyid/{id:int}",GetReportById).RequireAuthorization(r => r.RequireRole("User", "Checker", "Admin")).CacheOutput(t => t.Expire(TimeSpan.FromMinutes(5)).Tag("repid"));
            app.MapPatch("/update/{id:int}", UpdareReport).RequireAuthorization();
            app.MapDelete("/delete/{id:int}", DeleteReport).RequireAuthorization(r => r.RequireRole("User", "Checker", "Admin"));
            app.MapPost("/addreport", AddReport).RequireAuthorization(r => r.RequireRole("User", "Checker", "Admin"));
        }

        public static async Task<IResult> GetAllReports(ISender sender, string? name, string? snder, int lechid)
        {
 
            var reports = await sender.Send(new GetAllReports(name,snder,lechid));

            return Results.Ok(reports);
        }

        public static async Task<IResult> GetOwnReports(ISender sender, string? name, int lechid)
        {
            var ownreports = await sender.Send(new GetOwnReport(name,lechid));

            return Results.Ok(ownreports);
        }

        public static async Task<IResult> GetReportById(ISender sender, int id)
        {
            var currentrep = await sender.Send(new GetReportByIdQuery(id));

            return Results.Ok(currentrep);
        }

        public static async Task<IResult> UpdareReport(ISender sender, UpdateReportDTO dTO, int id, IOutputCacheStore cache)
        {
            var update = await sender.Send(new UpdateReportCommand(dTO, id));
            await cache.EvictByTagAsync("reps", new CancellationToken());
            await cache.EvictByTagAsync("ownreps", new CancellationToken());
            await cache.EvictByTagAsync("idreps", new CancellationToken());
            await cache.EvictByTagAsync("repid", new CancellationToken());
            return Results.Ok(update);
        }

        public static async Task<IResult> DeleteReport(ISender sender, int id, IOutputCacheStore cache)
        {
            await sender.Send(new DeleteReportCommand(id));
            await cache.EvictByTagAsync("reps", new CancellationToken());
            await cache.EvictByTagAsync("ownreps", new CancellationToken());
            await cache.EvictByTagAsync("idreps", new CancellationToken());
            await cache.EvictByTagAsync("repid", new CancellationToken());
            return Results.Ok("Удалено");
        }

        public static async Task<IResult> AddReport(ISender sender, CreateReportDTO dTO, IOutputCacheStore cache)
        {
            var result = await sender.Send(new AddReportCommand(dTO));
            await cache.EvictByTagAsync("reps", new CancellationToken());
            await cache.EvictByTagAsync("ownreps", new CancellationToken());
            await cache.EvictByTagAsync("idreps", new CancellationToken());
            await cache.EvictByTagAsync("repid", new CancellationToken());
            return Results.Ok(result);
        }

        public static async Task<IResult> GetReportsById(ISender sender, Guid uid, string? name, int lechid)
        {
            var result = await sender.Send(new GetReportsByIdUser(uid, name, lechid));

            return Results.Ok(result);
        }
    }
}
