using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrailerScope.Domain.Entities
{
	public class SearchTitleResult {
		[MinLength( 1 )] public string Title { get; set; } = "";
		public IEnumerable<MovieInfo> Movies { get; set; } = new List<MovieInfo>();
	}
}
