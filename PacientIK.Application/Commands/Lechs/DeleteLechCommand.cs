using MediatR;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Commands.Lechs
{
    public record DeleteLechCommand(int id) : IRequest;

    public class DeleteLechCommandHandler(ILechRepository rep) : IRequestHandler<DeleteLechCommand>
    {
        public async Task Handle(DeleteLechCommand command, CancellationToken cancellationToken)
        {
            await rep.DeleteLech(command.id);
        }
    }

}
