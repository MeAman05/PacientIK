using MediatR;
using Microsoft.AspNetCore.Http;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Commands.Lechs
{
    public record UpdateLechCommand(UpdateLechDTO DTO, int id) : IRequest<UpdateLechDTO>;

    public class UpdateLechCommandHandler(ILechRepository repository, IHttpContextAccessor http) : IRequestHandler<UpdateLechCommand, UpdateLechDTO>
    {
        public async Task<UpdateLechDTO> Handle(UpdateLechCommand command, CancellationToken token)
        {
            await repository.UpdateLech(command.id,command.DTO.ToUpdateLechEntity());

            return command.DTO;
        }
    }

}
