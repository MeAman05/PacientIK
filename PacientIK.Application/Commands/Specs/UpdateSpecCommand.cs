using MediatR;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Commands.Specs
{
    public record UpdateSpecCommand(UpdateSpecDTO DTO, int id) : IRequest<UpdateSpecDTO>;

    public class UpdateSpecCommandHandler(ISpecReposotory reposotory) : IRequestHandler<UpdateSpecCommand, UpdateSpecDTO>
    {
        public async Task<UpdateSpecDTO> Handle(UpdateSpecCommand cmd, CancellationToken token)
        {
            await reposotory.UpdateSpec(cmd.id, cmd.DTO.ToSpecUpdateEntity());

            return cmd.DTO;
        }
    }

}
