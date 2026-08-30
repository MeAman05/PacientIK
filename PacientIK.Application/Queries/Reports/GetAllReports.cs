using MediatR;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Queries.Reports
{
    public record GetAllReports(string? name, string? sender, int lechid) : IRequest<List<ReportDTO>>;

    public class GetAllReportsHandler(IReportRepository repository) : IRequestHandler<GetAllReports, List<ReportDTO>>
    {
        public async Task<List<ReportDTO>> Handle(GetAllReports request, CancellationToken token)
        {
            var rep = await repository.GetAllReports(request.name, request.sender, request.lechid);

            return rep.Select(r => r.ToReportDTO()).ToList();
        }
    }

}
