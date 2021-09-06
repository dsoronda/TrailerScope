using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;

namespace TrailerScope.Services.Caching
{
	public interface ISearchTitleCacheService : ICache<SearchTitleResult> {	}
}
