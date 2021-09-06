using System.ComponentModel.DataAnnotations;

namespace TrailerScope.Domain.Entities
{
	public class MovieRatings
	{
		[MinLength(2)] public string Site { get; set; } = "";
        
		public float Rating { get; set; }
        
	}
}
