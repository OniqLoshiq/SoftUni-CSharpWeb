using System.Linq;
using TORSHIA.App.Models.Binding;
using TORSHIA.Models;

namespace TORSHIA.App.Services.Contracts
{
    public interface ITaskService
    {
        void CreateTask(TaskCreateBindingModel model);

        Task GetTaskById(string id);

        void ReportTask(string taskId, string username);

        IQueryable<Task> GetAllUnreportedTasks();
    }
}
