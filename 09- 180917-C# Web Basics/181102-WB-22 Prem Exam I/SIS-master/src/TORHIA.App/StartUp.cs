using Microsoft.EntityFrameworkCore;
using SIS.Framework.Api;
using SIS.Framework.Services;
using TORSHIA.App.Services;
using TORSHIA.App.Services.Contracts;
using TORSHIA.Data;
using TORSHIA.Models;

namespace TORSHIA.App
{
    public class StartUp : MvcApplication
    {
        private void SeedData()
        {
            using (var ctx = new TorshiaDbContext())
            {
                ctx.Database.EnsureDeleted();

                ctx.Database.Migrate();

                ctx.Sectors.Add(new Sector { Name = "Customers" });
                ctx.Sectors.Add(new Sector { Name = "Marketing" });
                ctx.Sectors.Add(new Sector { Name = "Finances" });
                ctx.Sectors.Add(new Sector { Name = "Internal" });
                ctx.Sectors.Add(new Sector { Name = "Management" });

                ctx.SaveChanges();
            }
        }

        public override void Configure()
        {
            using (var ctx = new TorshiaDbContext())
            {
                System.Console.WriteLine("Wait several seconds to create and seed the database...");

                ctx.Database.EnsureDeleted();

                ctx.Database.Migrate();

                this.SeedData();

                ctx.SaveChanges();
            }
               
        }

        public override void ConfigureServices(IDependencyContainer dependencyContainer)
        {
           dependencyContainer.RegisterDependency<IUserService, UserService>();
           dependencyContainer.RegisterDependency<ITaskService, TaskService>();
           dependencyContainer.RegisterDependency<IReportService, ReportService>();

            base.ConfigureServices(dependencyContainer);
        }
    }
}
