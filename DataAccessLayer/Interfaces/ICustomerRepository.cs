using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface ICustomerRepository
    {
        // creating GetCustomersAsync inside the interface to be implemented
        Task<IEnumerable<Customer>> GetCustomersAsync();

    }
}
