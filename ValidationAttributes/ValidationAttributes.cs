using System.ComponentModel.DataAnnotations;
using VolgaIT2023.Models;

namespace VolgaIT2023.ValidationAttributes
{
    public class ExistingAttribute<T> : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = validationContext.GetService<DatabaseContext>();
            if (typeof(T) == typeof(Account)) {
                if (!(dbContext.Accounts.Any(c => c.Id == (long)value)))
                    return new ValidationResult("The user with this id must exists in database.");
            }
            if (typeof(T) == typeof(Transport))
            {
                if (!(dbContext.Transports.Any(c => c.Id == (long)value)))
                    return new ValidationResult("The transport with this id must exists in database.");
            }
            return ValidationResult.Success;
        }
    }
    public class UniqueUsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = validationContext.GetService<DatabaseContext>();
            if (dbContext.Accounts.Any(c => c.Username == (string)value))
            {
                return new ValidationResult("The username must be unique.");
            }
            return ValidationResult.Success;
        }
    }
}
