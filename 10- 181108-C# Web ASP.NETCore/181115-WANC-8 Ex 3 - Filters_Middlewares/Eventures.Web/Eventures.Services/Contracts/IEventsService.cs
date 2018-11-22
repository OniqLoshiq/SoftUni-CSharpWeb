using Eventures.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Eventures.Services.Contracts
{
    public interface IEventsService
    {
        Task CreateEventAsync(Event model);

        IQueryable<Event> GetAllEvents();
    }
}
