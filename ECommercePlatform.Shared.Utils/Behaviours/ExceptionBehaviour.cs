using MediatR;

using Microsoft.Extensions.Logging;

namespace ECommercePlatform.Shared.Utils.Behaviours;

public class ExceptionBehaviour<TRequest, TResponse>(ILogger logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "CleanArchitecture Request: Unhandled Exception for Request {Name} {@Request}",
                typeof(TRequest).Name, request);

            throw;
        }
    }
}
