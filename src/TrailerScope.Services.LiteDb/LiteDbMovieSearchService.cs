using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;
using TrailerScope.Services.Caching;

namespace TrailerScope.Services.LiteDb {
	public class LiteDbMovieSearchService : IMovieSearchService, ISearchTitleCacheService {
		private readonly LiteDbManager dbManager;

		public LiteDbMovieSearchService( LiteDbManager dbManager ) {
			this.dbManager = dbManager ?? throw new ArgumentNullException( nameof(dbManager) );
		}

		/// <summary>
		/// Get search results from DB
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<Result<IEnumerable<MovieInfo>>> SearchByTitleAsync( string title ) => Task.FromResult(
			this.Contains( title )
				? Result.Ok<IEnumerable<MovieInfo>>( GetItem( title ).Movies )
				: Result.Fail<IEnumerable<MovieInfo>>( "Not found" ) );

		public bool Contains( string key ) =>
			dbManager.SearchTitleResultcollection.Exists(
				x => x.Title.ToLowerInvariant().Equals( key.ToLowerInvariant() ) );

		public SearchTitleResult GetItem( string key ) => dbManager.SearchTitleResultcollection.FindOne(
			x => x.Title.ToLowerInvariant().Equals( key.ToLowerInvariant() ) );

		public void AddItem( string key, SearchTitleResult item ) =>
			dbManager.SearchTitleResultcollection.Insert( item.Title.ToLowerInvariant(), item );
	}
}
