using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.Identity
{
    public class BaseModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
