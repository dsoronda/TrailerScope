using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using TrailerScope.Domain.Entities;

namespace TrailerScope.Contracts.Services
{
    public interface IMovieSearchService
    {
        /// <summary>
        /// Search Movies by title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        Task<Result<IEnumerable<MovieInfo>>> SearchByTitleAsync(string title);

		Task<Result<MovieInfo>> GetMovieInfo( string imdbId );
	}
}
