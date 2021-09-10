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

		protected override async Task OnInitializedAsync() {
			if (string.IsNullOrWhiteSpace( imdb_id ))
				Snackbar.Add( $"Invalid submit triggerred", Severity.Error );

			FluentResults.Result<MovieInfo> result = await _searchService.GetMovieInfo( imdb_id );

			if (result.IsSuccess) {
				MovieInfoData = result.Value;
				Snackbar.Add( "Got movie info",Severity.Normal );
			} else {
				Snackbar.Add( $"{result.Reasons.First()}", Severity.Error );
			}
			//return base.OnInitializedAsync();
		}
	}
}
