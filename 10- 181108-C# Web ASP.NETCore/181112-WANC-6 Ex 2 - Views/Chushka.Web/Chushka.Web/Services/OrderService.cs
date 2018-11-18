using Chushka.Models;
using Chushka.Web.Data;
using Chushka.Web.Models.ViewModels;
using Chushka.Web.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chushka.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly ChushkaDbContext context;

        public OrderService(ChushkaDbContext context)
        {
            this.context = context;
        }

        public async Task<int> CreateOrder(string productId, string clientId)
        {
            Order order = new Order
            {
                OrderedOn = DateTime.UtcNow,
                ProductId = productId,
                ClientId = clientId
            };

            this.context.Orders.Add(order);

            try
            {
                int result = await this.context.SaveChangesAsync();
                return result;
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
        }

        public IEnumerable<OrdersAllViewModel> GetAllOrders()
        {
            return this.context.Orders.Select(o => new OrdersAllViewModel
            {
                Id = o.Id,
                OrderedOn = o.OrderedOn,
                ProductName = o.Product.Name,
                ClientName = o.Client.UserName
            });
        }
    }
}
