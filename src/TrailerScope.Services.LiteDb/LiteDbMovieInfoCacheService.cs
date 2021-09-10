using System;
using TrailerScope.Domain.Entities;
using TrailerScope.Services.Caching;

namespace TrailerScope.Services.LiteDb
{
	public class LiteDbMovieInfoCacheService :  IMovieInfoCacheService {
		private readonly ILiteDbManager dbManager;
		public LiteDbMovieInfoCacheService( ILiteDbManager dbManager ) {
			this.dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
		}

		public bool Contains( string key )  => dbManager.MovieInfoCollection.Exists( x => x.IMDbId.Equals( key ) );

		public MovieInfo GetItem( string key ) => dbManager.MovieInfoCollection.FindOne( x => x.IMDbId.Equals( key ) );

		public void AddItem( string key, MovieInfo item ) {
				item.Title = item.Title.ToLowerInvariant();
				dbManager.MovieInfoCollection.Insert( item );
		}
	}
}
