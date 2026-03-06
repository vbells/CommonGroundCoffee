using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IProductRepository
    {
        // creating GetProductsAsync inside the interface to be implemented
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product?> GetByIdAsync(int id);     // async version
     
    }
}
