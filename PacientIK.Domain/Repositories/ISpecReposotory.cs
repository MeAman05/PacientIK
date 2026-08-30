using PacientIK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Domain.Repositories
{
    public interface ISpecReposotory
    {
        Task<List<Spec>> GetAllSpeces();
        Task<Spec> GetSpecById(int id);
        Task UpdateSpec(int id, Spec spec);
        Task DeleteSpec(int id);
        Task AddSpec(Spec spec);
    }
}
