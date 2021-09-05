using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;

namespace TrailerScopeBlazorWasm.Server.Services {
	public class MemoryMovieInfoService :IMovieInfoService {
		private List<MovieInfo> movies = new() {
			new MovieInfo() {
				Title = "spider-man",
			},
			new MovieInfo() {
				Title = "batman",
			},
			new MovieInfo() {
				Title = "x-men",
			},
			new MovieInfo() {
				Title = "The batman",
				ReleaseYear = 2022,
			},
		};

		public Task<Result<IEnumerable<MovieInfo>>> SearchByTitleAsync( string title ) {
			var data= movies.Where( x =>
				x.Title.Contains( title, comparisonType: StringComparison.InvariantCultureIgnoreCase )
				);

			return Task.FromResult<Result<IEnumerable<MovieInfo>>>( data.Any()
				? Result.Ok( data )
				: Result.Fail<IEnumerable<MovieInfo>>( $"Nothing found for title : {title}" ) );
		}

	}
}
