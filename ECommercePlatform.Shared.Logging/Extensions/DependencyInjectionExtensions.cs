using ECommercePlatform.Shared.ServiceDefaults.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace ECommercePlatform.Shared.Logging.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddLogging(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<LoggerConfiguration>(builder.Configuration.GetSection(nameof(LoggerConfiguration)));
        Log.Logger = CreateLogger(builder.Configuration);
    }

    private static ILogger CreateLogger(IConfiguration configuration)
    {
        const string logstashDefaultUrl = "http://localhost:8080";
        
        var logstashUrl = configuration.Get<UrlsConfiguration>()?.LogstashUrl;
        
        return new Serilog.LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Http(logstashUrl ?? logstashDefaultUrl, queueLimitBytes: null)
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }
}
