using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace DataAccessLayer.Entities
{
        public class ApplicationUser : IdentityUser
        {
            // Optional: extra fields for your app
            public string? FirstName { get; set; }
            public string? LastName { get; set; }

            // Optional: link to your existing Customers table
            public int? CustomerID { get; set; }
        }
}
