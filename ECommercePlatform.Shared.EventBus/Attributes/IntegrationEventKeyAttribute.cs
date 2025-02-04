namespace ECommercePlatform.Shared.EventBus.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class IntegrationEventKeyAttribute(string key) : Attribute
{
    public string Key { get; } = key;
}
