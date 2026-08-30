using MediatR;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Commands.Reports
{
    public record DeleteReportCommand(int id) : IRequest;

    public class DeleteReportCOmmandHandler(IReportRepository repository) : IRequestHandler<DeleteReportCommand>
    {
        public async Task Handle(DeleteReportCommand command,CancellationToken token)
        {
            await repository.DeleteReport(command.id);
        }
    }

}
