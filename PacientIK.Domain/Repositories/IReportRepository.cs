using PacientIK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Domain.Repositories
{
    public interface IReportRepository
    {
        Task<List<Report>> GetAllReports(string? name, string? sender, int lechid);
        Task<Report> GetReportById(int Id);
        Task AddReport(Report report, List<int> lechIds);
        Task DeleteReport(int Id);
        Task ChangeReport(int id, Report report, List<int> newLechIds);
        Task<List<Report>> GetOwnReports(Guid uid, string? name, int lechid);
    }
}
