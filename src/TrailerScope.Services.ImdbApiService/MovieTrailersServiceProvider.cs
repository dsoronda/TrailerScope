using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using IMDbApiLib;
using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;

namespace TrailerScope.Services.ImdbApiService
{
    public class MovieTrailersServiceProvider : ITrailerService
    {
        private readonly string _apiKey;

        public MovieTrailersServiceProvider(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        private ApiLib GetApiLib() => new ApiLib(_apiKey);
        
        public Task<Result<IEnumerable<MovieInfo>>> GetTrailersForMovie(string moveId)
        {
            throw new NotImplementedException();
        }
        
        
    }
}