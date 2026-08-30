using MediatR;
using Microsoft.AspNetCore.Http;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace PacientIK.Application.Commands.Reports
{
    public record AddReportCommand(CreateReportDTO DTO) : IRequest<CreateReportDTO>;

    public class AddReportCommandHandler(IReportRepository repository, IHttpContextAccessor http) : IRequestHandler<AddReportCommand, CreateReportDTO>
    {
        public async Task<CreateReportDTO> Handle(AddReportCommand req, CancellationToken cancellationToken)
        {
            var user = http.HttpContext.User;
            var myid = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var map = req.DTO.ToAddReportEntity(myid);

            await repository.AddReport(map, req.DTO.lechids);

            return req.DTO;
        }
    }

}
