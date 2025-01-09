using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Validations
{
    public class FileValidationAttributeMaxSize(long maxSize) : ValidationAttribute
    {
        private readonly long _maxSize = maxSize;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
            {
                return ValidationResult.Success;
            }
            if (file.Length > _maxSize)
            {
                return new ValidationResult($"Image size exceeds {_maxSize / 1024 / 1024}MB.");
            }
            return ValidationResult.Success;

        }
    }
}
