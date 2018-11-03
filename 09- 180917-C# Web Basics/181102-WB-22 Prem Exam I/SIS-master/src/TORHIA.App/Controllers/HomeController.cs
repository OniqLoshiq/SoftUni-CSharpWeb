using SIS.Framework.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using TORSHIA.App.Models.View;
using TORSHIA.App.Services.Contracts;

namespace TORSHIA.App.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ITaskService taskService;

        public HomeController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        public IActionResult Index()
        {
            if(this.Identity != null)
            {
                this.Model["username"] = this.Identity.Username;

                List<IndexTaskViewModel> taskModels = this.taskService.GetAllUnreportedTasks()
                    .Select(t => new IndexTaskViewModel
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Level = t.Sectors.Count
                    }).ToList();

                List<IndexTaskRowViewModel> taskRowModel = new List<IndexTaskRowViewModel>();

                for (int i = 0; i < taskModels.Count; i++)
                {
                    if(i % 5 == 0)
                    {
                        taskRowModel.Add(new IndexTaskRowViewModel());
                    }

                    taskRowModel[taskRowModel.Count - 1].Tasks.Add(taskModels[i]);
                }

                this.Model["TaskRows"] = taskRowModel;

                if (this.Identity.Roles.Contains("Admin"))
                {
                    return this.View("IndexAdmin");
                }
                if (this.Identity.Roles.Contains("User"))
                {
                    return this.View("IndexUser");
                }
            }

            return this.View();
        }
    }
}
