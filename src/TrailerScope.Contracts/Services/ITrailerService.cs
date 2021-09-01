using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using TrailerScope.Domain.Entities;

namespace TrailerScope.Contracts.Services
{
    /// <summary>
    /// Service that provides Trailer for movies
    /// </summary>
    public interface ITrailerService
    {
        public Task<Result<IEnumerable<MovieInfo>>> GetTrailersForMovie(string moveId);
    }
}