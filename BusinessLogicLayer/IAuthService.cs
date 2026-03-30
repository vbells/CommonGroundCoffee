using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(string email, string password, string firstName,
            string lastName, string streetAddress, string city,
            string state, string zipCode, string phoneNumber);
        Task<Customer?> ValidateLoginAsync(string email, string password);
    }
}