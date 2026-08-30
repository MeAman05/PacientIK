using Microsoft.EntityFrameworkCore;
using PacientIK.Domain.Entities;
using PacientIK.Domain.Repositories;
using PacientIK.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Infrastructure.Repositories
{
    public class SpecRepository(ApplicationDbContext context) : ISpecReposotory
    {
        public async Task AddSpec(Spec spec)
        {
            await context.Spec.AddAsync(spec);

            await context.SaveChangesAsync();
        }

        public async Task DeleteSpec(int id)
        {
            var currentspec = await context.Spec.FirstOrDefaultAsync(s => s.Id == id);

            if(currentspec != null)
            {
                context.Spec.Remove(currentspec);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Spec>> GetAllSpeces()
        {
            var speces = await context.Spec.AsNoTracking().ToListAsync();

            return speces;
        }

        public async Task<Spec> GetSpecById(int id)
        {
           var currentspec = await context.Spec.FirstOrDefaultAsync(s => s.Id == id);

           if( currentspec == null)
            {
                return null;
            }

           return currentspec;
        }

        public async Task UpdateSpec(int id, Spec spec)
        {
            var currentspec = await context.Spec.FirstOrDefaultAsync(s => s.Id == id);

            if(currentspec != null)
            {
                currentspec.Name = spec.Name;
            }

            await context.SaveChangesAsync();
        }

    }
}
