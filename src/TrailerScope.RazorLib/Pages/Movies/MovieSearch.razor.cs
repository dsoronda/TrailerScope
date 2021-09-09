using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

using MudBlazor;

namespace TrailerScope.RazorLib.Pages.Movies {
	public partial class MovieSearch {

		[Inject] Services.IWasmMovieSearchApiService _searchService { get; set; }
		[Inject] ISnackbar Snackbar { get; set; }
		[Inject] ILogger<MovieSearch> logger { get; set; }


		internal SearchQuery model = new();
		internal class SearchQuery {
			[Required, MinLength( 1 )]
			internal string SearchText { get; set; }
		}

		IReadOnlyList<Domain.Entities.MovieInfo> movies;

		bool HaveMovies => movies != null && movies!.Any();

		private void InvalidSubmit( EditContext context ) {
			Snackbar.Add( $"Invalid submit trigered", Severity.Info );
		}

		private async Task OnValidSubmit( EditContext context ) {
			// note : this is workaround when invalid submit trigers validsubmit
			if (!context.Validate() || string.IsNullOrWhiteSpace( model.SearchText )) return;

			await SearchMovies( model.SearchText );
			//success = true;
			StateHasChanged();
		}

		private async Task SearchMovies( string title ) {
			logger.LogInformation( "got search request for title: {SearchText}", title );

			var result = await _searchService.SearchByTitleAsync( title );

			logger.LogInformation( "got response : {result.IsSuccess}", result.IsSuccess );

			if (result.IsSuccess && result.Value.Any()) {
				IReadOnlyList<Domain.Entities.MovieInfo> x = result.Value;
				movies = x;
				return;
			}

			if (result.IsFailed) {
				logger.LogInformation( "got reasons: {Reasons}", result.Reasons.First() );
				Snackbar.Add( $"Fail: Reason: {result.Reasons.First()}", Severity.Error );
			}

			// handle success with not found data
			Snackbar.Add( $"Nothing found", Severity.Warning );
		}

	}
}
