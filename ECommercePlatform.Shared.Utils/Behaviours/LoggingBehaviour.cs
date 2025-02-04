using MediatR.Pipeline;

using Microsoft.Extensions.Logging;
using System.Security.Claims;
using ECommercePlatform.Shared.ServiceDefaults.Authentication.Interfaces;

namespace ECommercePlatform.Shared.Utils.Behaviours;

public class LoggingBehaviour<TRequest>(IUserInfo userInfo, ILogger logger) : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var user = userInfo.Principal;
        var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        logger.LogInformation("Processing request '{RequestName}' for user '{UserId}'", typeof(TRequest).Name, userId);
        
        return Task.CompletedTask;
    }
}
