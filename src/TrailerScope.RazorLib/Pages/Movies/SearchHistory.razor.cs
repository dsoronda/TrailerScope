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
	public partial class SearchHistory {
		[Inject] Services.IWasmMovieSearchApiService _searchService { get; set; }

		[Inject] ILogger<MovieSearch> logger { get; set; }

		public List<SearchTitleResult> SearchResult { get; set; } = new();

		protected override async Task OnInitializedAsync() {
			Snackbar.Add( $"Fetching data.", Severity.Info );

			try {

				var result = await _searchService.GetAllSearches();
				if (result.IsFailed) {
					Snackbar.Add( $"Unable to retrive search history.", Severity.Error );
					return;
				}

				SearchResult.AddRange( result.Value );
				Snackbar.Add( $"Got {SearchResult.Count} items.", Severity.Success );

			} catch (Exception ex) {
				Snackbar.Add( $"Got exception {ex.Message}", Severity.Error );
			}

			foreach (var search in SearchResult) {
				var item = new TreeItemData( search.Title ) {
					Icon = Icons.Custom.Brands.Vimeo
				};

				foreach (var movieInfo in search.Movies) {
					var subItem = new TreeItemData( movieInfo.Title ) {
						Icon = Icons.Custom.Brands.YouTube,
						ImdbId = movieInfo.ImdbId,
						IsMovieInfo = true,
					};
					item.TreeItems.Add( subItem );
				}

				TreeItems.Add( item );
			}

			//return base.OnInitializedAsync();
		}


		private HashSet<TreeItemData> TreeItems { get; set; } = new();
		public class TreeItemData {
			public TreeItemData( string text ) { Text = text; }
			public string Text { get; set; }

			public string Icon { get; set; }

			public bool IsExpanded { get; set; } = false;

			public bool HasChild => TreeItems != null && TreeItems.Count > 0;

			public HashSet<TreeItemData> TreeItems { get; set; } = new HashSet<TreeItemData>();
			public string ImdbId { get; set; }
			public bool IsMovieInfo { get; set; } = false;

		}

	}
}
