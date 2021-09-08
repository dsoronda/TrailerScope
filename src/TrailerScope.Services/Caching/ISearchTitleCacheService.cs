using System.Collections.Generic;

using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;

namespace TrailerScope.Services.Caching
{
	public interface ISearchTitleCacheService : ICache<SearchTitleResult> {
		IEnumerable<string> GetCachedSearchTitles();
	}
}
