using MoviesApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="The Field with name {0} is required")]
        [StringLength(50)]
        [FirstLetterUppercase]
        public string Name { get; set; }
        public ICollection<MovieGenres> MovieGenres { get; set; }
    }
}
