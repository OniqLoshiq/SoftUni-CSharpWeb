using Eventures.Models;
using Eventures.Services.Contracts;
using Eventures.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Eventures.Web.Controllers
{
    public class OrdersController : Controller
    {
        private IOrdersService ordersService;

        private IEventsService eventsService;

        private UserManager<EventuresUser> userManager;

        public OrdersController(IOrdersService ordersService, IEventsService eventsService, UserManager<EventuresUser> userManager)
        {
            this.ordersService = ordersService;
            this.eventsService = eventsService;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var @event = this.eventsService.GetById(model.EventId);

                if(@event == null)
                {
                    return View();
                }

                var user = this.userManager.FindByNameAsync(this.User.Identity.Name).Result;

                var order = new Order
                {
                    Event = @event,
                    Customer = user,
                    OrderedOn = DateTime.Now,
                    TicketsCount = model.Tickets
                };

                await this.ordersService.CreateOrderAsync(order);

                return RedirectToAction("MyEvents", "Events");
            }

            return RedirectToAction("All", "Events");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult All()
        {
            var model = this.ordersService.GetAllOrders().ToList().Select(o => new OrderViewModel
            {
                EventName = o.Event.Name,
                CustomerName = o.Customer.UserName,
                OrderedOn = o.OrderedOn.ToString(@"dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture)
            });

            return View(model);
        }
    }
}
