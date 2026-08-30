using MediatR;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Commands.Lechs
{
    public record AddNewLechCommand(CreateLechDTO DTO) : IRequest<CreateLechDTO>;

    public class AddNewLechCommandHandler(ILechRepository repository) : IRequestHandler<AddNewLechCommand, CreateLechDTO>
    {
        public async Task<CreateLechDTO> Handle(AddNewLechCommand cmd, CancellationToken token)
        {
            await repository.AddLech(cmd.DTO.ToAddLechEntity());

            return cmd.DTO;
        }
    }

}
