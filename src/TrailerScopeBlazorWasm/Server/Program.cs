using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using TrailerScope.Contracts.Configuration;

namespace TrailerScopeBlazorWasm.Server {
	public class Program {
		public static void Main( string[] args ) {
			Log.Logger = new LoggerConfiguration()
				//.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Debug)
				.Enrich.FromLogContext()
				//.Enrich.WithCorrelationId()
				//.Enrich.WithProcessId()
				//.Enrich.WithThreadId()
				//.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] PID:{ProcessId} TID:{ThreadId} {Message:lj}{NewLine}{Exception}")
				.WriteTo.Console()
				.CreateLogger();

			var IMDbApiKey = Environment.GetEnvironmentVariable( "IMDbApiKey" );
			if (IMDbApiKey.IsNullOrEmpty()) throw new Exception( $"Missing {nameof(AppSettings.ImdbApiKey)}" );

			var litedBPath = TrailerScope.Services.LiteDb.ConnectionStringHelper.GetDbPath();
			if (litedBPath.IsNullOrEmpty()) throw new Exception( $"Missing {nameof(AppSettings.LiteDbConnectionString)}" );
			Log.Logger.Information( "Using db path : {litedBPath}", litedBPath );

			var other = new List<string>() {
				$"{nameof(AppSettings)}:{nameof(AppSettings.LiteDbConnectionString)}={litedBPath}",
				$"{nameof(AppSettings)}:{nameof(AppSettings.ImdbApiKey)}={IMDbApiKey}"
			};

			var builder = CreateHostBuilder( args, other.ToArray() );
			var host = builder.Build();
			Log.Logger.Information( "Starting host runner");

			host.Run();
			Log.Logger.Information( "Host started" );


		}

		public static IHostBuilder CreateHostBuilder( string[] args, string[] otherKeys ) =>
			Host.CreateDefaultBuilder( args )
				.UseSerilog()
				.ConfigureWebHostDefaults( webBuilder => {
					webBuilder.UseStartup<Startup>();
				} )
				.ConfigureAppConfiguration( ( hostingContext, config ) => {
					var env = hostingContext.HostingEnvironment;
					config.AddJsonFile( "appsettings.json", optional: false, reloadOnChange: true )
						.AddJsonFile( $"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true );
					config.AddEnvironmentVariables();

					if (args != null) config.AddCommandLine( args );

					if (otherKeys.Any()) config.AddCommandLine( otherKeys );
				} );
	}
}
