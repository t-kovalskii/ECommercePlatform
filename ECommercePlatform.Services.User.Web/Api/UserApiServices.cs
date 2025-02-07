using MediatR;

namespace ECommercePlatform.Services.User.Web.Api;

public class UserApiServices(ILogger logger, IMediator mediator)
{
    public ILogger Logger { get; } = logger;
    
    public IMediator Mediator { get; } = mediator;
}