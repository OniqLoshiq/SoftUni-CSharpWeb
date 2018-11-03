using System.Collections.Generic;
using System.Linq;
using TORSHIA.App.Models.Binding;
using TORSHIA.App.Models.View;
using TORSHIA.App.Services.Contracts;
using TORSHIA.Data;
using TORSHIA.Models;

namespace TORSHIA.App.Services
{
    public class TaskService : ITaskService
    {
        private readonly TorshiaDbContext context;
        private readonly IReportService reportService;


        public TaskService(TorshiaDbContext context, IReportService reportService)
        {
            this.context = context;
            this.reportService = reportService;
        }

        public void CreateTask(TaskCreateBindingModel model)
        {
            Task task = new Task
            {
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                Participants = model.Participants,
                IsReported = false
            };

            if(model.Sectors != null)
            {
                foreach (var sector in model.Sectors)
                {
                    task.Sectors.Add(new TaskSector
                    {
                        Sector = this.context.Sectors.SingleOrDefault(s => s.Name == sector),
                        Task = task
                    });
                }
            }

            this.context.Tasks.Add(task);
            this.context.SaveChanges();
        }

        public IQueryable<Task> GetAllUnreportedTasks()
        {
            return this.context.Tasks.Where(t => t.IsReported == false);
        }

        public Task GetTaskById(string id)
        {
            return this.context.Tasks.SingleOrDefault(t => t.Id == id);
        }

        public void ReportTask(string taskId, string username)
        {
            Task task = this.GetTaskById(taskId);

            if(task == null)
            {
                return;
            }

            task.IsReported = true;

            this.reportService.CreateReport(task.Id, username);

            this.context.SaveChanges();
        }
    }
}
