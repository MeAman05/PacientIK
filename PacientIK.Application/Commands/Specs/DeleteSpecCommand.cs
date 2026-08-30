using MediatR;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Commands.Specs
{
    public record DeleteSpecCommand(int id) : IRequest;

    public class DeleteSpecCommandHandler(ISpecReposotory reposotory) : IRequestHandler<DeleteSpecCommand>
    {
        public async Task Handle(DeleteSpecCommand command, CancellationToken cancellationToken)
        {
            await reposotory.DeleteSpec(command.id);
        }
    }

}
