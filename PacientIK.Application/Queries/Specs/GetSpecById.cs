using MediatR;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Queries.Specs
{
    public record GetSpecById(int id) : IRequest<SpecDTO>;

    public class GetSpecByIdHandler(ISpecReposotory reposotory) : IRequestHandler<GetSpecById, SpecDTO>
    {
        public async Task<SpecDTO> Handle(GetSpecById query, CancellationToken token)
        {
            var dto = await reposotory.GetSpecById(query.id);

            return dto.ToSpecDTO();
        }
    }

}
