using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using MudBlazor;

using TrailerScope.Domain.Entities;

namespace TrailerScope.RazorLib.Pages.Movies {
	public partial class MovieInfoPage {

		[Inject] Services.IWasmMovieSearchApiService _searchService { get; set; }
		[Inject] ILogger<MovieSearch> logger { get; set; }
		[Inject] NavigationManager navMan { get; set; }
		[Inject] ISnackbar Snackbar { get; set; }

		public MovieInfo MovieInfoData{ get; set; }

		[Parameter]public string imdb_id { get; set; }
		string h3_Title = "Movie info";
		bool movieNotFound = false;

		protected override async Task OnInitializedAsync() {
			if (string.IsNullOrWhiteSpace( imdb_id ))
				Snackbar.Add( $"Invalid submit triggerred", Severity.Error );
			try {

				FluentResults.Result<MovieInfo> result = await _searchService.GetMovieInfo( imdb_id );

				if (result.IsSuccess) {
					MovieInfoData = result.Value;
					h3_Title = $"{MovieInfoData.Title} ({MovieInfoData.ReleaseYear})";
					Snackbar.Add( "Got movie info", Severity.Normal );
				} else {
					movieNotFound = true;
					Snackbar.Add( $"{result.Reasons.First()}", Severity.Error );
				}
			} catch (Exception ex) {
				movieNotFound = true;
				logger.LogError( ex, $"Fetching data for {imdb_id} failed" );
			}
			//return base.OnInitializedAsync();
		}
	}
}
