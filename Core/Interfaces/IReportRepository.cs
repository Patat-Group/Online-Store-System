using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IReportRepository
    {
        public Task<IReadOnlyList<Report>> GetAll();
        public Task<IReadOnlyList<Report>> GetAllReportsByUserId(string userId);
        public Task<Report> GetReportById(int reportId);
        public Task<bool> DeleteReport(int reportId);
        public Task<bool> AddReport(Report report);
        public Task<DateTime> GetLastReportDate(string reporterUserId,string reportedUserId);
    }
}