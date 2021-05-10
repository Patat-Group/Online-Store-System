using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Services.Data
{
    public class ReportRepository :IReportRepository
    {
        private readonly StoreContext _context;

        public ReportRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<Report>> GetAll()
        {
            var result = await _context.Reports.ToListAsync();
            return result;
        }

        public async Task<IReadOnlyList<Report>> GetAllReportsByUserId(string userId)
        {
            var result = await _context.Reports.Where(x=>x.UserDestinationReportId==userId).ToListAsync();
            return result;
        }

        public async Task<Report> GetReportById(int id)
        {
            var result = await _context.Reports.SingleOrDefaultAsync(x=>x.Id==id);
            return result;
        }

        public async Task<bool> DeleteReport(int id)
        {
            var reportToDelete = await GetReportById(id);
            if (reportToDelete == null) return false;
            _context.Reports.Remove(reportToDelete);
            return await SaveChanges();
        }

        public async Task<bool> AddReport(Report report)
        {

            await _context.Reports.AddAsync(report);
            return await SaveChanges();
        }

        public async Task<DateTime> GetLastReportDate(string userReporterId, string userReportedId)
        {
            var lastReport = await _context.Reports.OrderByDescending(a => a.ReportDate).FirstOrDefaultAsync(x =>
                x.UserSourceReportId == userReporterId && x.UserDestinationReportId == userReportedId);
            return lastReport?.ReportDate ?? DateTime.MinValue;
        }
        private async Task<bool> SaveChanges()
        {
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
    }
}