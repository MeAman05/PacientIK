using MediatR;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Commands.Specs
{
    public record AddSepcCommand(CreateSpecDTO dto) : IRequest<CreateSpecDTO>;

    public class AddSepcCommandHandler(ISpecReposotory reposotory) : IRequestHandler<AddSepcCommand , CreateSpecDTO>
    {
        public async Task<CreateSpecDTO> Handle(AddSepcCommand req, CancellationToken token)
        {
            await reposotory.AddSpec(req.dto.ToSpecAddEntity());

            return req.dto;
        }
    }
}
