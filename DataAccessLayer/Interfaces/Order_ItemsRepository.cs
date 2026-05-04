using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public class Order_ItemsRepository : IOrder_ItemsRepository
    {   
        private readonly ApplicationDbContext _context;
    
            public Order_ItemsRepository(ApplicationDbContext applicationDbContext)
            {
                this._context = applicationDbContext;
            }
    
           public async Task<IEnumerable<Order_Items>> GetOrder_ItemsAsync()
            {
                return await _context.Order_Items.ToListAsync();
        }
    }
}
