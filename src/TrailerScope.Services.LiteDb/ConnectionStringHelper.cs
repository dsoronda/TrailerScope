using System;

namespace TrailerScope.Services.LiteDb
{
	public static class ConnectionStringHelper {
		public const string LiteDbEnviromentVariable = "TrailerScopeLiteDb";

		/// <summary>
		/// This is to get api key, on windows use enviroment variable
		/// On Linux , use imdb_api_key.txt in home folder
		/// </summary>
		/// <returns>API key or empty string</returns>
		public static string GetDbPath() =>
			!string.IsNullOrWhiteSpace( Environment.GetEnvironmentVariable( LiteDbEnviromentVariable ) )
				? Environment.GetEnvironmentVariable( LiteDbEnviromentVariable )
				: ( Environment.OSVersion.Platform == PlatformID.Unix )
					? $"{Environment.GetEnvironmentVariable( "HOME" )}/TrailerScopeLiteDb.litedb"
					: "TrailerScopeLiteDb.litedb";
	}
}
