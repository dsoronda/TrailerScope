using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using FluentResults;

using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;
using TrailerScope.Services.Caching;
using System.Linq;
using LiteDB;

namespace TrailerScope.Services.LiteDb {
	public class LiteDbMovieSearchService : IMovieSearchService, ISearchTitleCacheService {
		private readonly LiteDbManager dbManager;

		public LiteDbMovieSearchService( LiteDbManager dbManager ) {
			this.dbManager = dbManager ?? throw new ArgumentNullException( nameof( dbManager ) );
		}

		/// <summary>
		/// Get search results from DB
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<Result<IEnumerable<MovieInfo>>> SearchByTitleAsync( string title ) => Task.FromResult(
			this.Contains( title )
				? Result.Ok( GetItem( title ).Movies )
				: Result.Fail<IEnumerable<MovieInfo>>( "Not found" ) );

		public bool Contains( string key ) => dbManager.SearchTitleResultcollection.Exists( x => x.Title.Equals( key ) );

		public SearchTitleResult GetItem( string key ) => dbManager.SearchTitleResultcollection.FindOne(
			x => x.Title == key );

		public void AddItem( string key, SearchTitleResult item ) {
			item.Title = item.Title.ToLowerInvariant();
			dbManager.SearchTitleResultcollection.Insert( item );
		}

		public IEnumerable<SearchTitleResult> GetCachedSearchTitles() => dbManager.SearchTitleResultcollection.FindAll().ToList();

		public Task<Result<MovieInfo>> GetMovieInfo( string imdbId ) {
			var info = dbManager.MovieInfoCollection.FindOne( x => x.ImdbId == imdbId );

			return ( info is not null ) ? Task.FromResult( Result.Ok( info ) ) :
			Task.FromResult( Result.Fail<MovieInfo>( $"Movie with Imdb ID {imdbId} not in db" ) );
		}
	}
}
