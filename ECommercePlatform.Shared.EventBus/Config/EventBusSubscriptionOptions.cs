namespace ECommercePlatform.Shared.EventBus.Configuration;

public class EventBusSubscriptionOptions
{
    
    public Dictionary<string, Type> Subscriptions { get; } = new();
}
