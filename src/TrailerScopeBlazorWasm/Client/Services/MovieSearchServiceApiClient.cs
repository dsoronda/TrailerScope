using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentResults;
using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TrailerScope.RazorLib.Services;

namespace TrailerScopeBlazorWasm.Client.Services {

	public class MovieSearchServiceApiClient : IWasmMovieSearchApiService {
		private readonly Url serverBase;
		private readonly FlurlClient client;
		private readonly ILogger<MovieSearchServiceApiClient> logger;

		public MovieSearchServiceApiClient( Url serverBase, IServiceProvider serviceProvider ) {
			logger = serviceProvider.GetService<ILogger<MovieSearchServiceApiClient>>();

			//logger = serviceProvider.GetService(typeof(ILogger));

			//this.logger = logger ?? throw new ArgumentNullException( nameof( logger ) );
			this.serverBase = serverBase ?? throw new ArgumentNullException( nameof( serverBase ) );

			this.client = new FlurlClient( serverBase )
				// 	.WithOAUthBearerToken(token))
				// .Configure(settings => ...)
				;
		}

		public async Task<Result<IReadOnlyList<SearchTitleResult>>> GetAllSearches() {
			var url = client.BaseUrl.AppendPathSegment( "/api/v1/movies/search_history" );
			var webResult = await url.GetAsync();

			logger.LogInformation( $"Search history - got response {webResult.ResponseMessage} with statuscode {webResult.StatusCode}" );
			if (webResult.ResponseMessage.IsSuccessStatusCode) {
				return Result.Ok( await webResult.GetJsonAsync<IReadOnlyList<SearchTitleResult>>() );
			}
			return Result.Fail<IReadOnlyList<SearchTitleResult>>( webResult.ResponseMessage.ReasonPhrase ?? "" );
		}

		public async Task<Result<IReadOnlyList<MovieInfo>>> SearchByTitleAsync( string title ) {
			try {
				var url = client.BaseUrl
							   .AppendPathSegment( "/api/v1/movies" )
							   .SetQueryParam( "title", title );
				var webResult = await url.AllowHttpStatus( "404" ).GetAsync();
				// .GetJsonAsync<IEnumerable<MovieInfo>>()
				;

				if (webResult.StatusCode == (int) HttpStatusCode.NotFound) {
					return Result.Ok<IReadOnlyList<MovieInfo>>( new List<MovieInfo>() );
				}

				logger.LogInformation( $"MovieInfoServiceApiClient - got response {webResult.ResponseMessage} with statuscode {webResult.StatusCode}" );
				if (webResult.ResponseMessage.IsSuccessStatusCode) {
					return Result.Ok( await webResult.GetJsonAsync<IReadOnlyList<MovieInfo>>() );
				}
				return Result.Fail<IReadOnlyList<MovieInfo>>( webResult.ResponseMessage.ReasonPhrase ?? "" );
			}
			catch (FlurlHttpException ex)       //	} when (ex.Call. == System.Net.HttpStatusCode.Forbidden){
			{
				logger.LogError( ex, "MovieInfoServiceApiClient - Error while fetching data for title {title}", title );
				return Result.Fail<IReadOnlyList<MovieInfo>>( new Error( ex.Message ) );
			}
		}

	}
}
