using BCrypt.Net;
using MediatR;
using PacientIK.Application.DTOs;
using PacientIK.Application.SavePhotoFunc;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
namespace PacientIK.Application.Commands.Users
{
    public record AddUserCommand(CreateUserDTO DTO) : IRequest<CreateUserDTO>;

    public class AddUserCommandHandler(IUserRepository repository, SavePhoto photo, SavePhotoId id) : IRequestHandler<AddUserCommand, CreateUserDTO>
    {
        public async Task<CreateUserDTO> Handle(AddUserCommand cmd, CancellationToken token)
        {
            string uid = "";
            var photourl = await photo.AddPhoto(cmd.DTO.photo);
            var hashpwd = BCrypt.Net.BCrypt.HashPassword(cmd.DTO.pwd);
            await repository.AddUser(cmd.DTO.ToUserEntity(photourl,hashpwd, id.SavePhoto));

            return cmd.DTO;
        }
    }

}
