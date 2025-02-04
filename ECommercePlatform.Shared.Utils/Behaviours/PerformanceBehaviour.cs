using MediatR;

using Microsoft.Extensions.Logging;

using System.Diagnostics;

namespace ECommercePlatform.Shared.Utils.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse>(ILogger logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const int MillisecondsThreshold = 500;
    private readonly Stopwatch _timer = new();
    
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _timer.Start();
        
        var result = await next();
        
        _timer.Stop();
        
        var elapsedMilliseconds = _timer.ElapsedMilliseconds;
        if (elapsedMilliseconds > MillisecondsThreshold)
        {
            logger.LogWarning("Long running request '{RequestName}' took {ElapsedMilliseconds}ms; ",
                typeof(TRequest).Name,
                elapsedMilliseconds);
        }

        return result;
    }
}
