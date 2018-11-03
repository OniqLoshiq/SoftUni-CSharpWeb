using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Method;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TORSHIA.App.Models.Binding;
using TORSHIA.App.Models.View;
using TORSHIA.App.Services.Contracts;
using TORSHIA.Models;

namespace TORSHIA.App.Controllers
{
    public class TasksController : BaseController
    {
        private ITaskService taskService;

        public TasksController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [Authorize("Admin")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize("Admin")]
        public IActionResult Create(TaskCreateBindingModel model)
        {
            if (this.ModelState.IsValid != true)
            {
                return this.View();
            }

            this.taskService.CreateTask(model);

            return this.RedirectToAction("/Home/Index");
        }

        [Authorize]
        public IActionResult Details()
        {
            string taskId = this.Request.QueryData["id"].ToString();

            Task task = this.taskService.GetTaskById(taskId);

            if (task == null)
            {
                return this.RedirectToAction("/Home/Index");
            }

            List<string> sectors = task.Sectors.Select(s => s.Sector.Name).ToList();

            TaskDetailsViewModel model = new TaskDetailsViewModel
            {
                Title = task.Title,
                Description = task.Description,
                Participants = task.Participants,
                Level = task.Sectors.Count,
                DueDate = task.DueDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Sectors = string.Join(", ", sectors)
            };

            this.Model["Task"] = model;

            return this.View();
        }

        [Authorize]
        public IActionResult Report()
        {
            string taskId = this.Request.QueryData["id"].ToString();

            this.taskService.ReportTask(taskId, this.Identity.Username);

            return this.RedirectToAction("/Home/Index");
        }
    }
}
