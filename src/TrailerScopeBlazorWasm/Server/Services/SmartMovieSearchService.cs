using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using FluentResults;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;
using TrailerScope.Services.Caching;
using System.Linq;
using TrailerScope.Services.LiteDb;

namespace TrailerScopeBlazorWasm.Server.Services {
	public interface IMovieSearchApiService : IMovieSearchService { }

	public class SmartMovieSearchService : IMovieSearchService {
		private readonly IMovieSearchApiService movieSearchApiService;
		private readonly IMovieSearchCache movieSearchCache;
		private readonly IMovieInfoCacheService movieInfoCacheService;
		private readonly ILogger<SmartMovieSearchService> logger;

		public SmartMovieSearchService( IMovieSearchApiService movieSearchApiService, IMovieSearchCache movieSearchCache, IMovieInfoCacheService movieInfoCacheService, ILogger<SmartMovieSearchService> logger ) {
			this.movieSearchApiService = movieSearchApiService ?? throw new ArgumentNullException( nameof( movieSearchApiService ) );
			this.movieSearchCache = movieSearchCache ?? throw new ArgumentNullException( nameof( movieSearchCache ) );
			this.movieInfoCacheService = movieInfoCacheService ?? throw new ArgumentNullException(nameof(movieInfoCacheService));
			this.logger = logger ?? throw new ArgumentNullException( nameof( logger ) );
		}

		public IEnumerable<SearchTitleResult> GetAllSearches() => movieSearchCache.GetCachedSearchTitles().ToList();

		public async Task<Result<IEnumerable<MovieInfo>>> SearchByTitleAsync( string title ) {
			if (movieSearchCache.Contains( title )) {
				logger.LogInformation( $"{nameof( SmartMovieSearchService )} - {nameof( SmartMovieSearchService.SearchByTitleAsync )} : Cache hit for [{title}]" );
				return Result.Ok<IEnumerable<MovieInfo>>( movieSearchCache.GetItem( title ).Movies ).WithSuccess( "From cache" );
			}

			var apiResult = await movieSearchApiService.SearchByTitleAsync( title );
			if (apiResult.IsFailed) return apiResult;

			movieSearchCache.AddItem( title.ToLowerInvariant(), new SearchTitleResult() { Title = title.ToLowerInvariant(), Movies = apiResult.Value } );

			logger.LogInformation( $"{nameof( SmartMovieSearchService )} - {nameof( SmartMovieSearchService.SearchByTitleAsync )} : Cache miss for [{title}], data retrieved from API and stored into Cache" );

			return apiResult;
		}

		public async Task<Result<MovieInfo>> GetMovieInfo( string imdbId ) {
			if (movieInfoCacheService.Contains(imdbId )) {
				logger.LogInformation( $"{nameof(SmartMovieSearchService)} - {nameof(SmartMovieSearchService.GetMovieInfo)} : Cache hit for [{imdbId}]" );
				var x = movieInfoCacheService.GetItem( imdbId );
				return Result.Ok<MovieInfo>(x).WithSuccess( "From cache" );
			}

			var apiResult = await movieSearchApiService.GetMovieInfo( imdbId );
			if (apiResult.IsFailed) return apiResult;

			movieInfoCacheService.AddItem( imdbId.ToLowerInvariant(), apiResult.Value );

			logger.LogInformation( $"{nameof(SmartMovieSearchService)} - {nameof(SmartMovieSearchService.GetMovieInfo)} : Cache miss for [{imdbId}], data retrieved from API and stored into Cache" );
			return apiResult;
		}
	}

	public class MovieSearchApiService : TrailerScope.Services.ImdbApiService.MovieSearchServiceProvider, IMovieSearchApiService {
		public MovieSearchApiService( string apiKey ) : base( apiKey ) { }
	}

	public interface IMovieSearchCache : ISearchTitleCacheService { }

	public class MyMovieSearchCache : TrailerScope.Services.LiteDb.LiteDbMovieSearchService, IMovieSearchCache {
		public MyMovieSearchCache( ILiteDbManager dbManager ) : base( dbManager ) { }
	}



}
