using ECommercePlatform.Shared.ServiceDefaults.Configuration;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ECommercePlatform.Shared.ServiceDefaults.Extensions;

public static class DependencyInjectionExtensions
{
    private const string UrlsEnvVariablesPrefix = "urls";
    private const string OtelExporterUrlEnvVarName = "OTEL_EXPORTER_OTLP_ENDPOINT";
    
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder,
        ServiceConfiguration serviceConfiguration)
    {
        builder.Services.Configure<UrlsConfiguration>(builder.Configuration.GetSection(UrlsEnvVariablesPrefix));
        
        builder.Services.AddServiceDiscovery();
        
        builder.AddOpenTelemetry(serviceConfiguration);
        builder.AddHealthChecks();
        
        return builder;
    }

    private static IHostApplicationBuilder AddOpenTelemetry(this IHostApplicationBuilder builder,
        ServiceConfiguration serviceConfiguration)
    {
        builder.Logging.AddOpenTelemetry();
        
        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddPrometheusExporter();
            })
            .WithTracing(tracing =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    tracing.SetSampler(new AlwaysOnSampler());
                }

                tracing.AddAspNetCoreInstrumentation()
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceConfiguration.Name))
                    .AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddJaegerExporter();
            });
        
        builder.AddOpenTelemetryExporters();
        
        return builder;
    }

    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        var useOtelExporter = builder.Configuration[OtelExporterUrlEnvVarName] is not null;

        if (useOtelExporter)
        {
            builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter());
            builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter());
            builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());
        }
        else
        {
            builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddConsoleExporter());
            builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddConsoleExporter());
            builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddConsoleExporter());
        }
        
        return builder;
    }
    
    private static IHostApplicationBuilder AddHealthChecks(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"]);
        
        return builder;
    }

    public static WebApplication UseServiceDefaults(this WebApplication app)
    {
        app.MapPrometheusScrapingEndpoint();
        app.MapHealthChecks();
        
        return app;
    }

    private static WebApplication MapHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health");
        app.MapHealthChecks("/health/alive", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live")
        });
        
        return app;
    }
}
