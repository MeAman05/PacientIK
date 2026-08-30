using MediatR;
using PacientIK.Application.DTOs;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Queries.Users
{
    public record GetAllUsersQuery(string text) : IRequest<List<UserDTO>>;

    public class GetAllUsersQUeryHandler(IUserRepository repository) : IRequestHandler<GetAllUsersQuery, List<UserDTO>>
    {
        public async Task<List<UserDTO>> Handle(GetAllUsersQuery query, CancellationToken token)
        {
            var map = await repository.GetUsers(query.text);

            return map.Select(m => m.ToUserDTO()).ToList();
        }
    }

}
