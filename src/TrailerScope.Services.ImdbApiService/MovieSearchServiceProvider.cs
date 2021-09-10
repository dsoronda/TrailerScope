using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FluentResults;
using IMDbApiLib;
using IMDbApiLib.Models;
using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;
using System.Linq;
using Mapster;

namespace TrailerScope.Services.ImdbApiService {
	public class MovieSearchServiceProvider : IMovieSearchService {
		private readonly string _apiKey;

		public MovieSearchServiceProvider( string apiKey ) {
			_apiKey = apiKey ?? throw new ArgumentNullException( nameof( apiKey ) );
		}

		private ApiLib GetApiLib() => new ApiLib( _apiKey );

		public async Task<Result<IEnumerable<MovieInfo>>> SearchByTitleAsync( string title ) {
			var apiLib = GetApiLib();

			// Title Data
			var data = await apiLib.SearchTitleAsync( title );
			if (!string.IsNullOrWhiteSpace( data.ErrorMessage )) return Result.Fail<IEnumerable<MovieInfo>>( data.ErrorMessage );

			var list = data.ToMovieInfoList();
			return Result.Ok<IEnumerable<MovieInfo>>( list );
		}

		/// <summary>
		/// Get extra movie informatio from IMDb-API
		/// </summary>
		/// <param name="imdbId"></param>
		/// <returns></returns>
		public async Task<Result<MovieInfo>> GetMovieInfo( string imdbId ) {
			var apiLib = GetApiLib();

			// Title Data
			TrailerData data = await apiLib.TrailerAsync( imdbId );

			if (!string.IsNullOrWhiteSpace( data.ErrorMessage )) return Result.Fail<MovieInfo>( data.ErrorMessage );

			var x = data.ToMovieInfo();
			return Result.Ok( x );
		}
	}

	internal static class SearchDataToMovieInfoAdapter {
		public static IEnumerable<MovieInfo> ToMovieInfoList( this SearchData data ) {
			List<MovieInfo> list = new();

			foreach (var item in data.Results) {
				var movie = item.Adapt<MovieInfo>();
				movie.IMDbId = item.Id;
				movie.Poster = item.Image;
				movie.ReleaseYear = GetYearFromDescription(item.Description );

				list.Add( movie );
			}

			return list;
		}

		public static MovieInfo ToMovieInfo( this TrailerData data ) {
			var info = data.Adapt<MovieInfo>();
			//info.IMDbId = data.IMDbId;
			info.ReleaseYear = ( !string.IsNullOrWhiteSpace( data.Year ) ) ? Convert.ToInt32( data.Year ) : 0;
			info.Description = data.VideoDescription;
			info.TrailerInfo = data.Adapt<ImdbTrailerData>();
			return info;
		}

		internal static int GetYearFromDescription( string text ) {
			int year = 0;
			if (text.StartsWith( '(' ) && text.Contains( ')' )) {
				var substring = text.Substring( 0, text.IndexOf( ')' ) );
				var stringValue = substring.Replace( "(", "" );
				int.TryParse( stringValue, out year );
			}
			return year;
		}
	}
}
