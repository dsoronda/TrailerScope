using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;
using TrailerScope.Services.Caching;
using TrailerScope.Services.LiteDb;

namespace TrailerScopeBlazorWasm.Server.Services {
	public class SmartMovieSearchService : IMovieSearchService {
		private readonly IMovieSearchApiService movieSearchApiService;
		private readonly IMovieSearchCache movieSearchCache;

		public SmartMovieSearchService( IMovieSearchApiService movieSearchApiService, IMovieSearchCache movieSearchCache ) {
			this.movieSearchApiService = movieSearchApiService ?? throw new ArgumentNullException( nameof(movieSearchApiService) );
			this.movieSearchCache = movieSearchCache ?? throw new ArgumentNullException( nameof(movieSearchCache) );
		}

		public async Task<Result<IEnumerable<MovieInfo>>> SearchByTitleAsync( string title ) {
			if (movieSearchCache.Contains( title )) return  Result.Ok<IEnumerable<MovieInfo>>( movieSearchCache.GetItem( title ).Movies ) ;

			var  apiResult = await movieSearchApiService.SearchByTitleAsync( title );
			if (apiResult.IsFailed) return apiResult;

			movieSearchCache.AddItem(title.ToLowerInvariant(),new SearchTitleResult(){Movies = apiResult.Value});

			return apiResult;
		}
	}

	public interface IMovieSearchApiService : IMovieSearchService {
	}


	public class MovieSearchApiService : TrailerScope.Services.ImdbApiService.MovieSearchServiceProvider,IMovieSearchApiService {
		public MovieSearchApiService(string apiKey) : base(apiKey)
		{
		}
	}

	public interface IMovieSearchCache : ISearchTitleCacheService { }

	public class MyMovieSearchCache : TrailerScope.Services.LiteDb.LiteDbMovieSearchService, IMovieSearchCache {
		public MyMovieSearchCache(LiteDbManager dbManager) : base(dbManager)
		{
		}
	}

}
