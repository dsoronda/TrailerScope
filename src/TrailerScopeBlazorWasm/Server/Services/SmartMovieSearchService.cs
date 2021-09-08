using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using FluentResults;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;
using TrailerScope.Services.Caching;
using TrailerScope.Services.LiteDb;

namespace TrailerScopeBlazorWasm.Server.Services {
	public class SmartMovieSearchService : IMovieSearchService {
		private readonly IMovieSearchApiService movieSearchApiService;
		private readonly IMovieSearchCache movieSearchCache;
		private readonly ILogger<SmartMovieSearchService> logger;

		public SmartMovieSearchService( IMovieSearchApiService movieSearchApiService, IMovieSearchCache movieSearchCache , ILogger<SmartMovieSearchService> logger ) {
			this.movieSearchApiService = movieSearchApiService ?? throw new ArgumentNullException( nameof( movieSearchApiService ) );
			this.movieSearchCache = movieSearchCache ?? throw new ArgumentNullException( nameof( movieSearchCache ) );
			this.logger = logger ?? throw new ArgumentNullException( nameof( logger ) );
		}

		public async Task<Result<IEnumerable<MovieInfo>>> SearchByTitleAsync( string title ) {
			if (movieSearchCache.Contains( title )) {
				logger.LogInformation( $"{nameof(SmartMovieSearchService)} - {nameof(SmartMovieSearchService.SearchByTitleAsync)} : Cache hit for [{title}]" );
				return Result.Ok<IEnumerable<MovieInfo>>( movieSearchCache.GetItem( title ).Movies ).WithSuccess("From cache");
			}

			var apiResult = await movieSearchApiService.SearchByTitleAsync( title );
			if (apiResult.IsFailed) return apiResult;

			movieSearchCache.AddItem( title.ToLowerInvariant(), new SearchTitleResult() {Title= title.ToLowerInvariant(), Movies = apiResult.Value } );

			logger.LogInformation( $"{nameof( SmartMovieSearchService )} - {nameof( SmartMovieSearchService.SearchByTitleAsync )} : Cache miss for [{title}], data fetchs from API and stored into Cache" );

			return apiResult;
		}
	}

	public interface IMovieSearchApiService : IMovieSearchService {
	}


	public class MovieSearchApiService : TrailerScope.Services.ImdbApiService.MovieSearchServiceProvider, IMovieSearchApiService {
		public MovieSearchApiService( string apiKey ) : base( apiKey ) { }
	}

	public interface IMovieSearchCache : ISearchTitleCacheService { }

	public class MyMovieSearchCache : TrailerScope.Services.LiteDb.LiteDbMovieSearchService, IMovieSearchCache {
		public MyMovieSearchCache( LiteDbManager dbManager ) : base( dbManager ) { }
	}

}
