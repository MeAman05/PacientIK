using MediatR;
using PacientIK.Application.DTOs;
using PacientIK.Application.SavePhotoFunc;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Commands.Users
{
    public record UpdateUserCommand(UpdateUserDTO DTO, Guid id) : IRequest<UpdateUserDTO>;

    public class UpdateUserCommandHandler(IUserRepository repository, SavePhoto photo) : IRequestHandler<UpdateUserCommand, UpdateUserDTO>
    {
        public async Task<UpdateUserDTO> Handle(UpdateUserCommand req, CancellationToken token)
        {
            var currentuser = await repository.GetUserBYId(req.id);

            if(currentuser == null)
            {
                return null;
            }
            var newphoto = await photo.ChangePhoto(req.DTO.Photo, currentuser.PhotoName, token);

            await repository.UpdateUser(req.DTO.ToUpdatedUserEntity(newphoto, currentuser.PhotoName), req.id);

            return req.DTO;
        }
    }
}
