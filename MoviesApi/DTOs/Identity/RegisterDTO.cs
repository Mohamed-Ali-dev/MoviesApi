using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.Identity
{
    public class RegisterDTO   : BaseModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
