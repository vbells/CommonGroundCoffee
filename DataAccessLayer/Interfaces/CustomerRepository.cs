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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext applicationDbContext)
        {
            this._context = applicationDbContext;
        }

        //implementing GetCustomersAsync from interface
        public async Task<IEnumerable<Customer>> GetCustomersAsync()
            {
                return await _context.Customers.ToListAsync();
        }
    }
}
