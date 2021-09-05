using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;

namespace TrailerScopeBlazorWasm.Client.Services {
	public class MovieInfoServiceApiClient :IMovieInfoService{
		public Task<Result<IEnumerable<MovieInfo>>> SearchByTitleAsync( string title ) => throw new System.NotImplementedException();
	}
}
