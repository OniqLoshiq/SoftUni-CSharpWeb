using System.Collections.Generic;
using TORSHIA.Models;

namespace TORSHIA.App.Services.Contracts
{
    public interface IReportService
    {
        Report GetReportById(string id);

        ICollection<Report> AllReports();

        void CreateReport(string taskId, string username);
    }
}
