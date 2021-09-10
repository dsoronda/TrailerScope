using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrailerScope.Contracts.Configuration;
using TrailerScope.Contracts.Services;
using TrailerScope.Services.Caching;
using TrailerScope.Services.LiteDb;
using TrailerScopeBlazorWasm.Server.Services;

namespace TrailerScopeBlazorWasm.Server
{
	public static class ServicesHelper {
		public static void RegisterMyServices( IServiceCollection services, IConfiguration options ) {
			//using var serviceProvider = services.BuildServiceProvider( new ServiceProviderOptions() { } );
			AppSettings appSettings = new();
			options.GetSection(nameof(AppSettings)).Bind(appSettings);

			//services.AddSingleton<IMovieSearchService, Services.MemoryMovieSearchService>();
			//services.AddSingleton<IMovieSearchService, LiteDbMovieSearchService>();

			services.AddSingleton<IMovieSearchApiService>(new MovieSearchApiService(appSettings.ImdbApiKey));
			services.AddSingleton<ILiteDbManager>( new LiteDbManager(appSettings.LiteDbConnectionString));

			services.AddSingleton<IMovieSearchCache, MyMovieSearchCache>();//new MyMovieSearchCache(new LiteDbManager(appSettings.LiteDbConnectionString)));
			services.AddSingleton<IMovieInfoCacheService, LiteDbMovieInfoCacheService>();
			services.AddSingleton<IMovieSearchService, SmartMovieSearchService>();
		}
	}
}
