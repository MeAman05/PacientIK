using PacientIK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Application.DTOs
{
    public static class UserDTOExtensions
    {
        public static UserDTO ToUserDTO(this User user)
        {
            return new UserDTO(
                user.Id,
                user.Name,
                user.Surname,
                user.LastName,
                user.Age,
                user.PhotoUrl,
                user.Spec.Name,
                user.Role
                );
        }

        public static User ToUserEntity(this CreateUserDTO dto, string photourl, string hashpwd, string uid)
        {
            return new User
            {
                Name = dto.name,
                Surname = dto.surname,
                LastName = dto.lastname,
                Age = dto.age,
                SpecId = dto.spec,
                PhotoUrl = photourl,
                PhotoName = uid,
                Core = dto.code,
                Role = dto.role,
                Password = hashpwd
            };
        }

        public static User ToUpdatedUserEntity(this UpdateUserDTO dto, string? photourl, string? currenturl)
        {
            var user = new User
            {
                Name = dto.Name,
                Surname = dto.Surname,
                LastName = dto.Lastname,
                SpecId = dto.Spec,
                Age = dto.Age,
                PhotoUrl = photourl ?? currenturl,
                Role = dto.Role,
            };

            return user;
        }
    }
}
