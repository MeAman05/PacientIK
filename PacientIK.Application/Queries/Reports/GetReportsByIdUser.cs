using MediatR;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Queries.Reports
{
    public record GetReportsByIdUser(Guid id, string name, int lechid) : IRequest<List<ReportDTO>>;

    public class GetReportsByIdUserHandler(IReportRepository repository) : IRequestHandler<GetReportsByIdUser, List<ReportDTO>>
    {
        public async Task<List<ReportDTO>> Handle(GetReportsByIdUser req, CancellationToken token)
        {
            var rep = await repository.GetOwnReports(req.id, req.name, req.lechid);

            return rep.Select(m => m.ToReportDTO()).ToList();
        }
    }
}
