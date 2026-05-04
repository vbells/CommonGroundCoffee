using DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IProductService
    {
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task AddProductAsync(Product product);  
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}