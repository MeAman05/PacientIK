using PacientIK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Domain.Repositories
{
    public interface ILechRepository
    {
        Task<List<Lech>> GetLeches(int specid);
        Task<Lech> GetLechById(int id);
        Task UpdateLech(int id, Lech lech);
        Task DeleteLech(int id);
        Task AddLech(Lech lech);
    }
}
