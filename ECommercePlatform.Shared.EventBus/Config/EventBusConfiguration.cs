namespace ECommercePlatform.Shared.EventBus.Configuration;

public class EventBusConfiguration
{
    public string Host { get; set; }
    
    public int Port { get; set; }
    
    public string UserName { get; set; }
    
    public string Password { get; set; }
    
    public string ServiceName { get; set; }
    
    public int PublishingRetryCount { get; set; }
}