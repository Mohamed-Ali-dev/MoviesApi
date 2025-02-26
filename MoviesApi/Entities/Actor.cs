﻿using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Biography { get; set; }
        public string Picture { get; set; }
        public ICollection<MovieActors> MovieActors { get; set; }
    }
}
