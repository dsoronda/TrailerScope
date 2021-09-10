using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Runtime.Serialization;

namespace TrailerScope.Domain.Entities {
	public class MovieInfo {
		[Required, MinLength( 1 )] public string Title { get; set; } = "";

		[Required, MinLength( 1 )] public string IMDbId { get; set; } = "";

		public int ReleaseYear { get; set; } = 0;

		public string Description { get; set; } = "";

		public string Poster { get; set; } = "";

		public IEnumerable<string> TrailerUrls { get; private set; } = new List<string>();

		public float Ratings { get; set; }

		public ImdbTrailerData TrailerInfo { get; set; }

		public YoutubeTrailerData YtTrailerData { get; set; }
	}

	public class ImdbTrailerData {
		public string VideoId { get; set; }
		public string VideoTitle { get; set; }
		public string VideoDescription { get; set; }
		public string ThumbnailUrl { get; set; }
		public string UploadDate { get; set; }
		public string Link { get; set; }
		public string LinkEmbed { get; set; }
	}

	public class YoutubeTrailerData {
		public string VideoId { get; set; }
		public string VideoUrl { get; set; }
	}
}
