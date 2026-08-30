using Microsoft.EntityFrameworkCore;
using PacientIK.Domain.Entities;
using PacientIK.Domain.Repositories;
using PacientIK.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        public async Task AddUser(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid id)
        {
            var currentuser = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if(currentuser is not null)
            {
                context.Users.Remove(currentuser);
                await context.SaveChangesAsync();
            }
        }

        public async Task<User> GetUserBYId(Guid id)
        {
            var currentuser = await context.Users.Include(s => s.Spec).FirstOrDefaultAsync(u => u.Id == id);

            if(currentuser is null)
            {
                return null;
            }

            return currentuser;
        }

        public async Task<List<User>> GetUsers(string text)
        {
            var users = context.Users.Include(s => s.Spec).AsNoTracking();

            if(!string.IsNullOrEmpty(text.ToLower()))
            {
               users = users.Where(u => u.Name.ToLower().Contains(text.ToLower())  ||  u.Surname.ToLower().Contains(text.ToLower()));

            }

            return await users.ToListAsync();
        }

        public async Task UpdateUser(User user, Guid id)
        {
            var currentuser = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            var specid = user.SpecId;
            if(specid == 0)
            {
                specid = 1;
            }
            if(currentuser is not null)
            {
                currentuser.Name = user.Name;
                currentuser.Surname = user.Surname;
                currentuser.Age = user.Age;
                currentuser.LastName = user.LastName;
                currentuser.SpecId = specid;
                if(user.PhotoName != null)
                {
                    currentuser.PhotoName = user.PhotoName;
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
