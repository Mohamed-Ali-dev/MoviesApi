using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Validations
{
    public class FileValidationAttributeAllowedExtensions(string[] allowedExtensions) : ValidationAttribute
    {
        private readonly string[] _allowedExtensions = allowedExtensions;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
            {
                return ValidationResult.Success;
            }
            var extension = Path.GetExtension(file.FileName)?.ToLower();
            if (!_allowedExtensions.Contains(extension))
            {
                return new ValidationResult($"Invalid image format. Allowed: {string.Join(", ", _allowedExtensions)}.");
            }
            return ValidationResult.Success;

        }
    }
}
