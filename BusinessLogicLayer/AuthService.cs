using BC = BCrypt.Net.BCrypt;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace BusinessLogicLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerRepository _customerRepo;

        public AuthService(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<bool> RegisterAsync(string email, string password, string firstName,
            string lastName, string streetAddress, string city,
            string state, string zipCode, string phoneNumber)
        {
            if (await _customerRepo.EmailExistsAsync(email))
                return false;

            var customer = new Customer
            {
                Email = email.ToLower(),
                PasswordHash = BC.HashPassword(password),
                FirstName = firstName,
                LastName = lastName,
                StreetAddress = streetAddress,
                City = city,
                State = state,
                ZipCode = zipCode,
                PhoneNumber = phoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            await _customerRepo.AddAsync(customer);
            await _customerRepo.SaveChangesAsync();
            return true;
        }

        public async Task<Customer?> ValidateLoginAsync(string email, string password)
        {
            var customer = await _customerRepo.GetByEmailAsync(email);
            if (customer is null) return null;
            return BC.Verify(password, customer.PasswordHash) ? customer : null;
        }
    }
}