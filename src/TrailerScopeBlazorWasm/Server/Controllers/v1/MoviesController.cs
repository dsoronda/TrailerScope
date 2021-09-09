using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentResults;

using IdentityServer4.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;
using TrailerScope.Services.Caching;

namespace TrailerScopeBlazorWasm.Server.Controllers.v1 {
	// [Authorize]
	[ApiController]
	[Route( "/api/v1/[controller]" )]
	public class MoviesController : ControllerBase {
		private readonly IMovieSearchService movieSearchService;
		private readonly ISearchTitleCacheService cache;
		private readonly ILogger<MoviesController> logger;

		public MoviesController( IMovieSearchService movieSearchService, Services.IMovieSearchCache cache, ILogger<MoviesController> logger ) {
			this.movieSearchService = movieSearchService ?? throw new ArgumentNullException( nameof( movieSearchService ) );
			this.cache = cache ?? throw new ArgumentNullException( nameof( cache ) );
			this.logger = logger ?? throw new ArgumentNullException( nameof( logger ) );
		}

		[HttpGet("search_title")]
		public async Task<ActionResult<IEnumerable<MovieInfo>>> SearchByTitle( [FromQuery] string title ) {
			if (string.IsNullOrWhiteSpace( title )) return BadRequest( "Missing title" );

			logger.LogInformation( "Got search request for : {title}", title );
			var errorMsg = "Unable to fetch movies for search : {title}";

			try {
				var result = await movieSearchService.SearchByTitleAsync( title );
				if (result.IsSuccess) return Ok( result.Value );

				logger.LogError( errorMsg, title );
				return NotFound();
			} catch (Exception ex) {
				logger.LogError( ex, errorMsg, title );
				return StatusCode( StatusCodes.Status500InternalServerError, value: errorMsg );
			}
		}

		[HttpGet( "search_history" )]
		public ActionResult<IEnumerable<SearchTitleResult>> GetSearchList() => cache.GetCachedSearchTitles().ToList();

		[HttpGet( "{imdbId}" )]
		public async Task<ActionResult<MovieInfo>> GetMovieInfo([FromRoute] string imdbId ) {
			if (string.IsNullOrWhiteSpace( imdbId )) return BadRequest( "Empty imdb id" );

			Result<MovieInfo> result =  await movieSearchService.GetMovieInfo( imdbId );

			return new MovieInfo { ImdbId = imdbId, Description = "test" };
		}
	}
}
