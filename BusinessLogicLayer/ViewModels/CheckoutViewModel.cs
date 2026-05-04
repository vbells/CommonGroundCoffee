using System.ComponentModel.DataAnnotations;

namespace CommonGroundCoffee.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; } = string.Empty;

        // Shipping Address
        [Display(Name = "Use existing address")]
        public bool UseExistingAddress { get; set; } = true;

        [Required(ErrorMessage = "Street address is required.")]
        public string StreetAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Zip code is required.")]
        public string ZipCode { get; set; } = string.Empty;

        // Payment Info
        [Required(ErrorMessage = "Cardholder name is required.")]
        [Display(Name = "Name on Card")]
        public string CardholderName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Card number is required.")]
        [CreditCard(ErrorMessage = "Invalid card number.")]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Expiration month is required.")]
        [Range(1, 12, ErrorMessage = "Invalid month.")]
        [Display(Name = "Expiration Month")]
        public int ExpirationMonth { get; set; }

        [Required(ErrorMessage = "Expiration year is required.")]
        [Range(2025, 2050, ErrorMessage = "Invalid year.")]
        [Display(Name = "Expiration Year")]
        public int ExpirationYear { get; set; }

        [Required(ErrorMessage = "CVV is required.")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "CVV must be 3-4 digits.")]
        [Display(Name = "CVV")]
        public string CVV { get; set; } = string.Empty;
    }
}