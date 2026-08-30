using MediatR;
using Microsoft.AspNetCore.Http;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace PacientIK.Application.Queries.Lechs
{
    public record GetAllLechesQuery() : IRequest<List<LechDTO>>;

    public class GetAllLechesQueryHandler(ILechRepository repository, IHttpContextAccessor http) : IRequestHandler<GetAllLechesQuery, List<LechDTO>>
    {
        public async Task<List<LechDTO>> Handle(GetAllLechesQuery query, CancellationToken token)
        {
            var user = http.HttpContext.User;
            var specIdString = http.HttpContext?.User?.FindFirst("specId")?.Value;

            var leches = await repository.GetLeches(Convert.ToInt32(specIdString));

            return leches.Select(m => m.ToLechDTO()).ToList();
        }
    }
}
