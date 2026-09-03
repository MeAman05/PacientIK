using Microsoft.EntityFrameworkCore;
using PacientIK.Domain.Entities;
using PacientIK.Domain.Repositories;
using PacientIK.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Infrastructure.Repositories
{
    public class ReportRepository(ApplicationDbContext context) : IReportRepository
    {
        public async Task AddReport(Report report, List<int> lechIds)
        {
            report.Leches = await context.Leches.Where(l => lechIds.Contains(l.Id)).ToListAsync();

            await context.Reports.AddAsync(report);
            await context.SaveChangesAsync();
        }

        public async Task ChangeReport(int id, Report report, List<int> newLechIds)
        {
            var currentreport = await context.Reports.Include(r => r.Leches).FirstOrDefaultAsync(r => r.Id == id);
            
            if(currentreport != null)
            {
                currentreport.PacientName = report.PacientName;
                currentreport.Price = report.Price;
                currentreport.Period = report.Period;
                currentreport.Leches = await context.Leches.Where(l => newLechIds.Contains(l.Id)).ToListAsync();

                await context.SaveChangesAsync();
            }

           
        }

        public async Task DeleteReport(int Id)
        {
            var currentreport = await context.Reports.FirstOrDefaultAsync(r => r.Id == Id);

            if( currentreport != null)
            {
               context.Reports.Remove(currentreport);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Report>> GetAllReports(string? name, string? sender, int lechid)
        {
            var reports = context.Reports.Include(i => i.Sender).Include(i => i.Leches).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                reports = reports.Where(r => r.PacientName.ToLower().Contains(name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(sender))
            {
                reports = reports.Where(r => r.Sender.Name.ToLower().Contains(sender.ToLower()) || 
                r.Sender.Surname.ToLower().Contains(sender.ToLower()));
            }
            if(lechid != 0)
            {
                reports = reports.Where(r => r.Leches.Any(l => l.Id == lechid));
            }
            return await reports.ToListAsync();
        }



        public async Task<List<Report>> GetOwnReports(Guid uid, string? name, int lechid)
        {
            var ownreport = context.Reports.Include(i => i.Sender).Include(i => i.Leches).AsNoTracking();

            ownreport = ownreport.Where(r => r.SenderId == uid);

            if (!string.IsNullOrWhiteSpace(name))
            {
                ownreport = ownreport.Where(r => r.PacientName.Trim().ToLower().Contains(name.Trim().ToLower()));
            }
            if(lechid != 0)
            {
                ownreport = ownreport.Where(r => r.Leches.Any(l => l.Id == lechid));
            }
            return await ownreport.ToListAsync();
        }

        public async Task<Report> GetReportById(int Id)
        {
            var currentreport = await context.Reports.Include(i => i.Sender).Include(i => i.Leches).FirstOrDefaultAsync(r => r.Id == Id);

            if(currentreport == null)
            {
                return null;
            }

            return currentreport;
        }
    }
}
