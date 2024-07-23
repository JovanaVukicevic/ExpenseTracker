using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ExpenseTracker.Service.Dto;

public class PasswordValidationAttribute : ValidationAttribute
{
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    {
        if (value == null) return new ValidationResult("Password is required.");

        var password = value.ToString();
#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
#pragma warning disable CS8604 // Possible null reference argument.
        if (!Regex.IsMatch(password, @"[A-Z]"))
            return new ValidationResult("The password must contain at least one uppercase letter.");
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
        if (!Regex.IsMatch(password, @"[a-z]"))
            return new ValidationResult("The password must contain at least one lowercase letter.");
        if (!Regex.IsMatch(password, @"\d"))
            return new ValidationResult("The password must contain at least one digit.");
        if (!Regex.IsMatch(password, @"[\W_]"))
            return new ValidationResult("The password must contain at least one non-alphanumeric character.");

#pragma warning disable CS8603 // Possible null reference return.
        return ValidationResult.Success;
#pragma warning restore CS8603 // Possible null reference return.
    }
}