using PacientIK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers(string text);
        Task<User> GetUserBYId(Guid id);
        Task AddUser(User user);
        Task UpdateUser(User user, Guid id);
        Task DeleteUser(Guid id);
    }
}
