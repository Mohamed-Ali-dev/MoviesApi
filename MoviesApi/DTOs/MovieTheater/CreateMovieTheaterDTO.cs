﻿using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.MovieTheater
{
    public class CreateMovieTheaterDTO
    {
        [Required]
        [StringLength(maximumLength: 75)]
        public string Name { get; set; }
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]

        public double Longitude { get; set; }
    }
}
