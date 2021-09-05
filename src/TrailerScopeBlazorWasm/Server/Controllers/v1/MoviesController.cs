using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrailerScope.Contracts.Services;

namespace TrailerScopeBlazorWasm.Server.Controllers.v1 {
	// [Authorize]
	[ApiController]
	[Route( "/api/v1/[controller]" )]
	public class MoviesController : ControllerBase {
		private readonly IMovieInfoService movieInfoService;
		private readonly ILogger<MoviesController> logger;

		public MoviesController( IMovieInfoService movieInfoService, ILogger<MoviesController> logger ) {
			this.movieInfoService = movieInfoService ?? throw new ArgumentNullException( nameof(movieInfoService) );
			this.logger = logger ?? throw new ArgumentNullException( nameof(logger) );
		}

		[HttpGet]
		public async Task<ActionResult> SearchByTitle( [FromQuery] string title ) {
			if (string.IsNullOrWhiteSpace( title )) return BadRequest( "Missing title" );

			logger.LogInformation( "Got search request for : {title}", title );
			var errorMsg = "Unable to fetch movies for search : {title}";

			try {
				var result = await movieInfoService.SearchByTitleAsync( title );
				if (result.IsSuccess) return Ok( result.Value );

				logger.LogError( errorMsg, title );
				return NotFound();
			} catch (Exception ex) {
				logger.LogError(ex, errorMsg, title );
				return StatusCode( StatusCodes.Status500InternalServerError, value: errorMsg );
			}
		}
	}
}
