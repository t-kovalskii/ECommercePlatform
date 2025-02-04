namespace ECommercePlatform.Services.User.Domain.Exceptions;

public class UserServiceDomainException : Exception
{
    public UserServiceDomainException(string message) : base(message) {}

    public UserServiceDomainException(string message, Exception innerException) : base(message, innerException) {}
}