using System;
using LiteDB;
using TrailerScope.Contracts.Services;
using TrailerScope.Domain.Entities;

namespace TrailerScope.Services.LiteDb
{
    public class LiteDbManager
    {
	    private readonly string dbPath;
	    public ILiteCollection<MovieInfo> MovieInfoCollection { get; private set; }
	    public ILiteCollection<SearchTitleResult> SearchTitleResultcollection { get; private set; }

	    public LiteDbManager( string dbPath ) {
		    this.dbPath = string.IsNullOrWhiteSpace( dbPath )
			    ? throw new ArgumentNullException( nameof(dbPath) )
			    : dbPath;

		    using var db = new LiteDatabase( dbPath );

		    // Get a collection (or create, if doesn't exist)
		    MovieInfoCollection = db.GetCollection<MovieInfo>(nameof(MovieInfo));
		    // Index document using document Name property
		    MovieInfoCollection.EnsureIndex(x => x.Title);


		    // Get a collection (or create, if doesn't exist)
		    SearchTitleResultcollection = db.GetCollection<SearchTitleResult>(nameof(MovieInfo));
		    // Index document using document Name property
		    SearchTitleResultcollection.EnsureIndex(x => x.Title);
	    }
    }
}
