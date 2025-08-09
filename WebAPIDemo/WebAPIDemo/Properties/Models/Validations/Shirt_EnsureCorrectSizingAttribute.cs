
using System.ComponentModel.DataAnnotations;

namespace WebAPIDemo.Properties.Models.Validations
{
    public class Shirt_EnsureCorrectSizingAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var shirt = validationContext.ObjectInstance as Shirt;

            if (shirt == null || string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult("Size is required and cannot be empty.");
            }

            if (!int.TryParse(shirt.Size, out int sizeValue))
            {
                return new ValidationResult("Size must be a valid number.");
            }

            if (string.IsNullOrWhiteSpace(shirt.Gender))
            {
                return new ValidationResult("Gender is required for size validation.");
            }

            // Validate size based on gender
            switch (shirt.Gender.ToLower())
            {
                case "men":
                    if (sizeValue < 6)
                    {
                        return new ValidationResult("For men's shirts, the size must be greater than or equal to 6.");
                    }
                    break;
                case "women":
                    if (sizeValue < 6)
                    {
                        return new ValidationResult("For women's shirts, the size must be greater than or equal to 6.");
                    }
                    break;
                default:
                    return new ValidationResult("Gender must be 'Men' or 'Women'.");
            }

            return ValidationResult.Success;
        }
    }
}