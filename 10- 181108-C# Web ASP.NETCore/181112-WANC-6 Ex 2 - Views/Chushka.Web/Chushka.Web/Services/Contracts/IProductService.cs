using Chushka.Models;
using Chushka.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chushka.Web.Services.Contracts
{
    public interface IProductService
    {
        Task<int> CreateProcuct(ProductCreateViewModel model);

        bool ExistsProduct(string productId);

        Product GetProductById(string id);

        Task<int> Edit(ProductUpdateDeleteViewModel model);

        void Delete(string id);

        IEnumerable<ProductPartialViewModel> GetAllProducts();
    }
}
