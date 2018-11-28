using Eventures.Models;
using Eventures.Services.Contracts;
using Eventures.Web.Filters;
using Eventures.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Eventures.Web.Controllers
{
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    public class EventsController : Controller
    {
        private readonly IEventsService eventsService;

        private readonly ILogger logger;

        public EventsController(IEventsService eventsService, ILoggerFactory loggerFactory)
        {
            this.eventsService = eventsService;
            this.logger = loggerFactory.CreateLogger(typeof(EventsController));
        }

        public IActionResult All()
        {
            var model = this.eventsService.GetAllEvents().ToList().Select(e => new EventViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Start = e.Start.ToString(@"dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture),
                End = e.End.ToString(@"dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture)
            });

            return View(model);
        }

        [Authorize(Roles = ("Admin"))]
        public IActionResult Create()
        {
            return View();
        }

        [ServiceFilter(typeof(LogEventCreateActionFilter))]
        [Authorize(Roles = ("Admin"))]
        [HttpPost]
        public async Task<IActionResult> Create(CreateEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var eventureEvent = new Event
                {
                    Name = model.Name,
                    Place = model.Place,
                    Start = model.Start,
                    End = model.End,
                    TotalTickets = model.TotalTickets,
                    TicketPrice = model.TicketPrice
                };

                await this.eventsService.CreateEventAsync(eventureEvent);

                logger.LogInformation($"--- Event created: {eventureEvent.Name} ---");

                return RedirectToAction("All", "Events");
            }

            return View();
        }

        [Authorize]
        public IActionResult MyEvents()
        {
            var model = this.eventsService.GetMyEvents(this.User.Identity.Name).Select(o => new EventWithTicketsViewModel
            {
                Name = o.Event.Name,
                Start = o.Event.Start.ToString(@"dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture),
                End = o.Event.End.ToString(@"dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture),
                Tickets = o.TicketsCount
            });

            return View(model);
        }
    }
}

