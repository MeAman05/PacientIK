using MediatR;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Queries.Reports
{
    public record GetReportByIdQuery(int id) : IRequest<ReportDTO>;

    public class GetReportByIdHandler(IReportRepository repository) : IRequestHandler<GetReportByIdQuery , ReportDTO>
    {
        public async Task<ReportDTO> Handle(GetReportByIdQuery request, CancellationToken token)
        {
            var rep = await repository.GetReportById(request.id);

            return rep.ToReportDTO();
        }
    }


}
