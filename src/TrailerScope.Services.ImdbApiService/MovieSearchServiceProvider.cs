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

namespace TrailerScope.Services.ImdbApiService {
	public class MovieSearchServiceProvider : IMovieSearchService {
		private readonly string _apiKey;

		public MovieSearchServiceProvider( string apiKey ) {
			_apiKey = apiKey ?? throw new ArgumentNullException( nameof( apiKey ) );
		}

		private ApiLib GetApiLib() => new ApiLib( _apiKey );

		public async Task<Result<IEnumerable<MovieInfo>>> SearchByTitleAsync( string title ) {
			// throw new NotImplementedException();
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
		public Task<Result<MovieInfo>> GetMovieInfo( string imdbId ) {
			// TODO : check api usage and implement this
			throw new NotImplementedException();
		}
	}

	internal static class SearchDataToMovieInfoAdapter {
		public static IEnumerable<MovieInfo> ToMovieInfoList( this SearchData data ) => data.Results
			.Select( x => new MovieInfo() {
				ImdbId = x.Id,
				Title = x.Title,
				Description = x.Description,
				Poster = x.Image,
				ReleaseYear = ( x.Description.StartsWith( '(' ) && x.Description.Contains( ')' ) )
					? Convert.ToInt32( x.Description.Substring( 0, x.Description.IndexOf( ')' ) ).Replace( "(", "" ) )
					: 0
			} ).ToList();
	}
}
