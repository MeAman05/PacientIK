using MediatR;
using Microsoft.AspNetCore.Http;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace PacientIK.Application.Queries.Reports
{
    public record GetOwnReport(string? name) : IRequest<List<ReportDTO>>;

    public class GetOwnReportHandler(IReportRepository repository, IHttpContextAccessor accessor) : IRequestHandler<GetOwnReport, List<ReportDTO>>
    {
        public async Task<List<ReportDTO>> Handle(GetOwnReport req, CancellationToken token)
        {
            var user = accessor.HttpContext.User;
            var myid = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var rep = await repository.GetOwnReports(myid,req.name);
            return rep.Select(m => m.ToReportDTO()).ToList();
        }
    }

}
