using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace TrailerScopeBlazorWasm.Client.Services {
	public class MovieInfoServiceApiClient : IMovieInfoService {
		private readonly Url serverBase;
		private readonly FlurlClient client;
		private readonly ILogger<MovieInfoServiceApiClient> logger;

		public MovieInfoServiceApiClient( Url serverBase, IServiceProvider serviceProvider ) {
			logger = serviceProvider.GetService<ILogger<MovieInfoServiceApiClient>>();

			//logger = serviceProvider.GetService(typeof(ILogger));

			//this.logger = logger ?? throw new ArgumentNullException( nameof( logger ) );
			this.serverBase = serverBase ?? throw new ArgumentNullException( nameof( serverBase ) );
			
			this.client = new FlurlClient( serverBase )
				// 	.WithOAUthBearerToken(token))
				// .Configure(settings => ...)
				;
		}

		public async Task<Result<IEnumerable<MovieInfo>>> SearchByTitleAsync( string title ) {
			try {
				Url bla = client.BaseUrl
							   .AppendPathSegment( "/api/v1/movies" )
							   .SetQueryParam( "title", title );
				var webResult = await bla.AllowHttpStatus( "404" ).GetAsync();
				// .GetJsonAsync<IEnumerable<MovieInfo>>()
				;

				logger.LogInformation( $"MovieInfoServiceApiClient - got response {webResult.ResponseMessage} with statuscode {webResult.StatusCode}" );
				if (webResult.ResponseMessage.IsSuccessStatusCode) {
					return Result.Ok<IEnumerable<MovieInfo>>( await webResult.GetJsonAsync<IEnumerable<MovieInfo>>() );
				}
				return Result.Fail<IEnumerable<MovieInfo>>( webResult.ResponseMessage.ReasonPhrase ?? "" );
			} catch (System.Exception ex) {
				logger.LogError( ex, "MovieInfoServiceApiClient - Error whie fetching data for title {title}", title );
				return Result.Fail<IEnumerable<MovieInfo>>( new Error() );
			}

		}
	}
}
