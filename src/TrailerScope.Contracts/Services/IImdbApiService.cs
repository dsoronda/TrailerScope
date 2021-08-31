using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using TrailerScope.Domain.Entities;

namespace TrailerScope.Contracts.Services
{
    public interface IImdbApiService
    {
        public Task<Result<IEnumerable<MovieInfo>>> GetMoviesByTitleAsync(string title);

    }
}