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
				//.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				//.Enrich.WithCorrelationId()
				//.Enrich.WithProcessId()
				//.Enrich.WithThreadId()
				//.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] PID:{ProcessId} TID:{ThreadId} {Message:lj}{NewLine}{Exception}")
				.CreateLogger();

			var IMDbApiKey = Environment.GetEnvironmentVariable( "IMDbApiKey" );
			if (IMDbApiKey.IsNullOrEmpty()) throw new Exception( $"Missing {nameof(AppSettings.ImdbApiKey)}" );

			var litedBPath = TrailerScope.Services.LiteDb.ConnectionStringHelper.GetDbPath();
			if (litedBPath.IsNullOrEmpty()) throw new Exception( $"Missing {nameof(AppSettings.LiteDbConnectionString)}" );

			var other = new List<string>() {
				$"{nameof(AppSettings.LiteDbConnectionString)}:{litedBPath}",
				$"{nameof(AppSettings.ImdbApiKey)}:{IMDbApiKey}"
			};

			var builder = CreateHostBuilder( args, other.ToArray() );
			return;

			builder.Build().Run();
		}

		public static IHostBuilder CreateHostBuilder( string[] args, string[] otherKeys ) =>
			Host.CreateDefaultBuilder( args )
				.UseSerilog()
				.ConfigureWebHostDefaults( webBuilder => {
					webBuilder.UseStartup<Startup>();
				} )
				.ConfigureAppConfiguration( ( hostingContext, config ) => {
					var env = hostingContext.HostingEnvironment;
					config.AddJsonFile( "appsettings.json", optional: true, reloadOnChange: true )
						.AddJsonFile( $"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true );
					config.AddEnvironmentVariables();

					if (args != null) config.AddCommandLine( args );

					if (otherKeys.Any()) config.AddCommandLine( otherKeys );
				} );
	}
}
