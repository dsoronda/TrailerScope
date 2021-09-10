using System;

using LiteDB;

using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;

namespace TrailerScope.Services.LiteDb {
	public interface ILiteDbManager
	{
		ILiteCollection<MovieInfo> MovieInfoCollection { get; }
		ILiteCollection<SearchTitleResult> SearchTitleResultCollection { get; }
	}

	public class LiteDbManager :  IDisposable, ILiteDbManager {
		private readonly string dbPath;
		private readonly LiteDatabase liteDbDatabase;

		public ILiteCollection<MovieInfo> MovieInfoCollection { get; private set; }
		public ILiteCollection<SearchTitleResult> SearchTitleResultCollection { get; private set; }

		public LiteDbManager( string dbPath ) {
			this.dbPath = string.IsNullOrWhiteSpace( dbPath )
				? throw new ArgumentNullException( nameof( dbPath ) )
				: dbPath;

			var litedbConnectionString = new ConnectionString( dbPath ) {
				//Password = "1234",
				Connection = ConnectionType.Shared
			};
			liteDbDatabase = new LiteDatabase( litedbConnectionString );

			// Get a collection (or create, if doesn't exist)
			MovieInfoCollection = liteDbDatabase.GetCollection<MovieInfo>( nameof( MovieInfo ) );
			// Index document using document Name property
			MovieInfoCollection.EnsureIndex( x => x.Title, unique: false );

			// Get a collection (or create, if doesn't exist)
			SearchTitleResultCollection = liteDbDatabase.GetCollection<SearchTitleResult>( nameof( SearchTitleResult ) );
			// Index document using document Name property
			SearchTitleResultCollection.EnsureIndex( x => x.Title, unique: true );
		}

		public void Dispose() => liteDbDatabase.Dispose();
	}


}
