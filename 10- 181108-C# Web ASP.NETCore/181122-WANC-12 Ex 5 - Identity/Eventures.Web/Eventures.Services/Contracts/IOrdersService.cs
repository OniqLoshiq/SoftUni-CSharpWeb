using Eventures.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Eventures.Services.Contracts
{
    public interface IOrdersService
    {
        Task CreateOrderAsync(Order model);

        IQueryable<Order> GetAllOrders();
    }
}
