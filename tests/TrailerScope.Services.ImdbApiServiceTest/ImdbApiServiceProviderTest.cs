using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Constraints;
using TrailerScope.Domain.Entities;
using TrailerScope.Services.ImdbApiService;

namespace TrailerScope.Services.ImdbApiServiceUnitTest {
	public class ImdbApiServiceProviderTest {
		private string api_key = "";

		private readonly string linux_api_key_file = $"{Environment.GetEnvironmentVariable( "HOME" )}//imdb_api_key.txt";

		[SetUp]
		public void Setup() {
			api_key = GetApiKey();
		}

		/// <summary>
		/// This is to get api key, on windows use enviroment variable
		/// On Linux , use imdb_api_key.txt in home folder
		/// </summary>
		/// <returns>API key or empty string</returns>
		private string GetApiKey() {
			if (Environment.OSVersion.Platform == PlatformID.Unix)
				return File.Exists( linux_api_key_file ) ? File.ReadAllText( linux_api_key_file ) : String.Empty;
			else
				return Environment.GetEnvironmentVariable( "imdb_api_key" ) ?? String.Empty;
		}


		[Test]
		public void CanGetApiKey() {
			api_key.Should().NotBeNullOrEmpty();
		}

		[Test]
		public async Task ServiceProvider_FindBy() {
			var movie_title = "spider-man";

			var provider = new MovieSearchServiceProvider( this.api_key );
			var result = await provider.SearchByTitleAsync( movie_title );
			result.IsSuccess.Should().BeTrue();

			result.Value.Should()
				.Contain( x => x.Title.Contains( movie_title, StringComparison.InvariantCultureIgnoreCase ) );
		}


		[Test]
		public async Task Get_MovieInfo_By_IMDbID() {
			// TODO : implement test
			var movie_imdb_id = "tt0145487"; // spiderman

			var provider = new MovieSearchServiceProvider( this.api_key );
			var result = await provider.GetMovieInfo( movie_imdb_id );
			result.IsSuccess.Should().BeTrue();

			MovieInfo movie = result.Value;
			movie.IMDbId.Should().Be( movie_imdb_id );
			movie.TrailerInfo.Should().NotBeNull();
		}
	}
}
