using DataAccessLayer.Interfaces;
using DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _productRepository.GetProductsAsync();
        }
    }
}