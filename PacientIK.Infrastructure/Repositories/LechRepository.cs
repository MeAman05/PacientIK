using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using PacientIK.Domain.Entities;
using PacientIK.Domain.Repositories;
using PacientIK.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Infrastructure.Repositories
{
    public class LechRepository(ApplicationDbContext db) : ILechRepository
    {
        public async Task AddLech(Lech lech)
        {
            await db.Leches.AddAsync(lech);
            await db.SaveChangesAsync();
        }

        public async Task DeleteLech(int id)
        {
            var currentlech = await db.Leches.Include(s => s.Spec).FirstOrDefaultAsync(l => l.Id == id);

            if(currentlech != null)
            {
                db.Leches.Remove(currentlech);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Lech> GetLechById(int id)
        {
            var currentlech = await db.Leches.Include(s => s.Spec).FirstOrDefaultAsync(l => l.Id == id);

            return currentlech;

        }

        public async Task<List<Lech>> GetLeches(int specid)
        {
            var leches = db.Leches.Include(s => s.Spec).Include(r => r.Reports).AsNoTracking();

            if(specid > 1)
            {
                leches = leches.Where(l =>  l.SpecId == specid);
                return await leches.ToListAsync();
            }
            else
            {
                return await leches.ToListAsync();
            }

            
        }

        public async Task UpdateLech(int id, Lech lech)
        {
            var currentlech = await db.Leches.Include(s => s.Spec).FirstOrDefaultAsync(l => l.Id == id);

            if( currentlech != null)
            {
                currentlech.Name = lech.Name;
                currentlech.SpecId = lech.SpecId;
            }

            await db.SaveChangesAsync();
        }
    }
}
