using DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IProductService
    {
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync();
    }
}