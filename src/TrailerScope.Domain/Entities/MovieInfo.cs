using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrailerScope.Domain.Entities
{
    public class MovieInfo
    {
        [Required, MinLength(1)] public string Title { get; init; } = "";

        [Required, MinLength(1)] public string ImdbId { get; init; } = "";

        public int? ReleaseYear { get; set; }
        
        public string Description { get; set; } = "";
        
        public string Poster { get; set; } = "";

        public IEnumerable<string> TrailerUrls { get; private set; } = new List<string>();

        public float Ratings { get; set; }
    }

    public class MovieRatings
    {
        [MinLength(2)] public string Site { get; set; } = "";
        
        public float Rating { get; set; }
        
    }
}