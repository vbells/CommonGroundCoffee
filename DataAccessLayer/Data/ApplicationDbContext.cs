using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        // db context that will be used to connect to Azure SQL database
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        // DbSet for Customers table is sql
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Orders> Orders { get; set; }

    }
}
