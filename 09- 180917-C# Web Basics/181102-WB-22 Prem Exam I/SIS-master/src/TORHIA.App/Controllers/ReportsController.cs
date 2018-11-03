using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TORSHIA.App.Models.View;
using TORSHIA.App.Services.Contracts;
using TORSHIA.Models;

namespace TORSHIA.App.Controllers
{
    public class ReportsController : BaseController
    {
        private IReportService reportService;

        public ReportsController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [Authorize("Admin")]
        public IActionResult All()
        {
            int indexCounter = 1;
            List<ReportsAllViewModel> allReports = this.reportService.AllReports()
                .Select(r => new ReportsAllViewModel
                {
                    Id = r.Id,
                    Index = indexCounter++,
                    Title = r.Task.Title,
                    Level = r.Task.Sectors.Count,
                    Status = r.Status.ToString()
                }).ToList();

            this.Model["AllReports"] = allReports;

            return this.View();
        }

        [Authorize("Admin")]
        public IActionResult Details()
        {
            string reportId = this.Request.QueryData["id"].ToString();

            Report report = this.reportService.GetReportById(reportId);

            if(report == null)
            {
                return this.RedirectToAction("Reports/All");
            }

            List<string> sectors = report.Task.Sectors.Select(s => s.Sector.Name).ToList();

            ReportDetailsViewModel model = new ReportDetailsViewModel
            {
                Id = report.Id,
                Title = report.Task.Title,
                Description = report.Task.Description,
                Level = report.Task.Sectors.Count,
                Participants = report.Task.Participants,
                DueDate = report.Task.DueDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                ReportedOn = report.ReportedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Status = report.Status.ToString(),
                Reporter = report.Reporter.Username,
                Sectors = string.Join(", ", sectors)
            };

            this.Model["Report"] = model;

            return this.View();
        }
    }
}
