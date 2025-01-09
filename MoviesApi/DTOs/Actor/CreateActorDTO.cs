using MoviesApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.Actor
{
    public class CreateActorDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Biography { get; set; }
        [FileValidationAttributeAllowedExtensions([".jpg", ".jpeg", ".png", ".gif"])]
        [FileValidationAttributeMaxSize(2 * 1024 * 1024)]
        public IFormFile? Picture { get; set; }
    }
}
