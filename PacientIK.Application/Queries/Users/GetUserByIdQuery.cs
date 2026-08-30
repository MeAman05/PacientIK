using MediatR;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Queries.Users
{
    public record GetUserByIdQuery(Guid id) : IRequest<UserDTO>;

    public class GetUserByIdQueryHandler(IUserRepository repository) : IRequestHandler<GetUserByIdQuery, UserDTO>
    {
        public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken token)
        {
            var map = await repository.GetUserBYId(request.id);

            return map.ToUserDTO();
        }
    }

}
