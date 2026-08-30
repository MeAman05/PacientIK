using MediatR;
using PacientIK.Application.SavePhotoFunc;
using PacientIK.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.Commands.Users
{
    public record DeleteUserCommand(Guid id) : IRequest;

    public class DeleteUserCommandHandler(IUserRepository repository, SavePhoto photo) : IRequestHandler<DeleteUserCommand>
    {
        public async Task Handle(DeleteUserCommand command ,CancellationToken cancellationToken)
        {
            var currentuser = await repository.GetUserBYId(command.id);
            string pid = currentuser.PhotoName.ToString();
            if(currentuser == null)
            {
                throw new Exception("is null");
            }
            await photo.DeletePhoto(pid.ToString());
            await repository.DeleteUser(command.id);
        }
    }

}
