using Chushka.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chushka.Web.Services.Contracts
{
    public interface IOrderService
    {
        Task<int> CreateOrder(string productId, string clientId);

        IEnumerable<OrdersAllViewModel> GetAllOrders();
    }
}
