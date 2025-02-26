﻿using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Entities
{
    public class MovieActors
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        [StringLength(maximumLength:75)]
        public string Character { get; set; }
        public int Order { get; set; }
        public Movie Movie { get; set; }
        public Actor Actor { get; set; }
    }
}
