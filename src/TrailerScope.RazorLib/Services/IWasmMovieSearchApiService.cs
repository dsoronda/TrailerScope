using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentResults;

using TrailerScope.Domain.Entities;

namespace TrailerScope.RazorLib.Services {
	public interface IWasmMovieSearchApiService {
		Task<Result<IReadOnlyList<SearchTitleResult>>> GetAllSearches();
		Task<Result<IReadOnlyList<MovieInfo>>> SearchByTitleAsync( string title );
	}
}
