using MediatR;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Queries.Specs
{
    public record GetAllSpecQuery() : IRequest<List<SpecDTO>>;

    public class GetAllSpecQueryHandler(ISpecReposotory reposotory) : IRequestHandler<GetAllSpecQuery, List<SpecDTO>>
    {
        public async Task<List<SpecDTO>> Handle(GetAllSpecQuery query, CancellationToken token)
        {
            var speces = await reposotory.GetAllSpeces();

            return speces.Select(m => m.ToSpecDTO()).ToList();
        }
    }

}
