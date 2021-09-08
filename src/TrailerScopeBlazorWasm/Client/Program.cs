using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using TrailerScope.Contracts.Services;
using TrailerScopeBlazorWasm.Client.Services;
using Serilog;
using Serilog.Debugging;
using TrailerScope.RazorLib.Services;

namespace TrailerScopeBlazorWasm.Client {
	public class Program {
		public static async Task Main( string[] args ) {
			// setup serilog

			SelfLog.Enable( m => Console.Error.WriteLine( m ) );

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.BrowserConsole()
				.CreateLogger();

			try {
				var builder = WebAssemblyHostBuilder.CreateDefault( args );
				builder.RootComponents.Add<App>( "#app" );

				RegisterServices( builder );

				await builder.Build().RunAsync();
			 } catch (Exception ex) {
				Log.Fatal( ex, "An exception occurred while creating the WASM host" );
				throw;
			}
		}

		private static void RegisterServices( WebAssemblyHostBuilder builder ) {
			builder.Services.AddHttpClient( "TrailerScopeBlazorWasm.ServerAPI", client => client.BaseAddress = new Uri( builder.HostEnvironment.BaseAddress ) )
				.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

			// Supply HttpClient instances that include access tokens when making requests to the server project
			builder.Services.AddScoped( sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient( "TrailerScopeBlazorWasm.ServerAPI" ) );

			builder.Services.AddApiAuthorization();
			ServiceProvider x = builder.Services.BuildServiceProvider();

			builder.Services.AddSingleton<IWasmMovieSearchApiService>( new MovieSearchServiceApiClient( new Uri( builder.HostEnvironment.BaseAddress ), x ) );


			builder.Services.AddMudServices();

		}
	}
}
