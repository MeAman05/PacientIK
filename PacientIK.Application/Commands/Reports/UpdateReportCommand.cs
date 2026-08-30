using MediatR;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Commands.Reports
{
    public record UpdateReportCommand(UpdateReportDTO DTO, int id) : IRequest<UpdateReportDTO>;

    public class UpdateReportCommandHandler(IReportRepository repository) : IRequestHandler<UpdateReportCommand, UpdateReportDTO>
    {
        public async Task<UpdateReportDTO> Handle(UpdateReportCommand req, CancellationToken token)
        {
            var map = req.DTO.ToUpdateReportEntity();

            await repository.ChangeReport(req.id, map, req.DTO.lechids);

            return req.DTO;
        }
    }

}
