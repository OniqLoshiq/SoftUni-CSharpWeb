using System;
using System.Collections.Generic;
using System.Linq;
using TORSHIA.App.Services.Contracts;
using TORSHIA.Data;
using TORSHIA.Models;
using TORSHIA.Models.Enums;

namespace TORSHIA.App.Services
{
    public class ReportService : IReportService
    {
        private readonly TorshiaDbContext context;

        public ReportService(TorshiaDbContext context)
        {
            this.context = context;
        }

        public void CreateReport(string taskId, string username)
        {
            Report report = new Report {
                Task = this.context.Tasks.First(t => t.Id == taskId),
                Reporter = this.context.Users.First(u => u.Username == username),
                ReportedOn = DateTime.UtcNow,
                Status = new Random().Next(1,5) == 4 ? Status.Archived : Status.Completed
            };

            this.context.Reports.Add(report);

            this.context.SaveChanges();
        }

        public ICollection<Report> AllReports()
        {
            return this.context.Reports.ToList();
        }

        public Report GetReportById(string id)
        {
            return this.context.Reports.SingleOrDefault(r => r.Id == id);
        }
    }
}
