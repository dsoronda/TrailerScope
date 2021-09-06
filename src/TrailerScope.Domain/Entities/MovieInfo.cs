using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Runtime.Serialization;

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
}
