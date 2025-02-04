using System.Diagnostics;

using OpenTelemetry.Context.Propagation;

namespace ECommercePlatform.Shared.EventBus.Configuration;

public class RabbitMqEventBusTelemetryOptions
{
    public const string ActivitySourceName = "EventBusRabbitMq";
    
    public ActivitySource ActivitySource { get; } = new(ActivitySourceName);
    
    public TextMapPropagator Propagator { get; } = Propagators.DefaultTextMapPropagator;
}
