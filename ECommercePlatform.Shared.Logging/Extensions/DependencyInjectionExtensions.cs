using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace ECommercePlatform.Shared.Logging.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddLogger(this IHostApplicationBuilder builder)
    {
        Log.Logger = CreateLogger(builder.Configuration);
    }

    private static ILogger CreateLogger(IConfiguration configuration)
    {
        const string logstashDefaultUrl = "http://localhost:8080";
        
        var loggerConfiguration = configuration.Get<LoggerConfiguration>();
        
        return new Serilog.LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Http(loggerConfiguration?.LogstashUrl ?? logstashDefaultUrl, queueLimitBytes: null)
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }
}
