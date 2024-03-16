using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Assignment_task_2.Models
{
    public class Customer
    {
       
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string contact { get; set; }
        public string address { get; set; }

        [EmailAddress]
        [UniqueEmail]
        public string email { get; set; }
        public string password { get; set; }
        public string cfmpassword { get; set; }

    }

    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (DataBaseConnect)validationContext.GetService(typeof(DataBaseConnect));

            var existingUser = dbContext.Customers.FirstOrDefault(u => u.email == (string)value);

            if (existingUser != null)
            {
                return new ValidationResult("The email address is already in use.");
            }

            return ValidationResult.Success;
        }
    }
}
