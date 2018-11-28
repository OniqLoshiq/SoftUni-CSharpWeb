using Eventures.Services.Contracts;
using Eventures.Data;
using Eventures.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Eventures.Services
{
    public class EventsService : IEventsService
    {
        private readonly EventuresDbContext db;

        private UserManager<EventuresUser> userManager;

        public EventsService(EventuresDbContext db, UserManager<EventuresUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task CreateEventAsync(Event model)
        {
            this.db.Events.Add(model);

            await this.db.SaveChangesAsync();
        }

        public IQueryable<Event> GetAllEvents()
        {
            return this.db.Events.Select(e => e);
        }

        public Event GetById(string id)
        {
            return this.db.Events.FirstOrDefault(e => e.Id == id);
        }

        public IQueryable<Order> GetMyEvents(string username)
        {
            return this.db.Orders.Where(o => o.Customer.UserName == username);
        }
    }
}
