using System.Linq;
using System.Threading.Tasks;
using Eventures.Data;
using Eventures.Models;
using Eventures.Services.Contracts;

namespace Eventures.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly EventuresDbContext db;

        public OrdersService(EventuresDbContext db)
        {
            this.db = db;
        }

        public async Task CreateOrderAsync(Order model)
        {
            this.db.Orders.Add(model);

            await this.db.SaveChangesAsync();
        }

        public IQueryable<Order> GetAllOrders()
        {
            return this.db.Orders;
        }
    }
}
