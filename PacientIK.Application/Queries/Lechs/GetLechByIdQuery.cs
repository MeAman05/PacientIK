using MediatR;
using Microsoft.AspNetCore.Http;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.WebRequestMethods;

namespace PacientIK.Application.Queries.Lechs
{
    public record GetLechByIdQuery(int id) : IRequest<LechDTO>;

    public class GetLechByIdQueryHandler(ILechRepository repository, IHttpContextAccessor http) : IRequestHandler<GetLechByIdQuery, LechDTO>
    {
        public async Task<LechDTO> Handle(GetLechByIdQuery request, CancellationToken token)
        {
            var user = http.HttpContext.User;
            var specIdString = http.HttpContext?.User?.FindFirst("specId")?.Value;

            var lech = await repository.GetLechById(request.id);

            return lech.ToLechDTO();
        }
    }

}
