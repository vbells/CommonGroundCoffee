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

        // existing
        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        // ── Auth methods ──────────────────────────────────────────────
        public async Task<Customer?> GetByEmailAsync(string email) =>
            await _context.Customers.FirstOrDefaultAsync(c => c.Email == email.ToLower());

        public async Task<bool> EmailExistsAsync(string email) =>
            await _context.Customers.AnyAsync(c => c.Email == email.ToLower());

        public async Task AddAsync(Customer customer) =>
            await _context.Customers.AddAsync(customer);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();

        public async Task<Customer?> GetByIdAsync(int customerId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Customer_ID == customerId);
        }
    }
}