using Eventures.Services.Contracts;
using Eventures.Data;
using Eventures.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Eventures.Services
{
    public class EventsService : IEventsService
    {
        private readonly EventuresDbContext db;

        public EventsService(EventuresDbContext db)
        {
            this.db = db;
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
    }
}
