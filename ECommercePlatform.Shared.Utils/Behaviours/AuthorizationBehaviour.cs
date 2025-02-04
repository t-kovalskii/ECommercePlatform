using ECommercePlatform.Shared.ServiceDefaults.Authentication.Interfaces;

using MediatR;

namespace ECommercePlatform.Shared.Utils.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse>(
    ICredentialsProvider credentialsProvider,
    IUserInfo userInfo,
    IAuthenticationValidator authenticationValidator) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var credentials = credentialsProvider.Credentials;
        var principal = await authenticationValidator.ValidateAsync(credentials);
        
        if (principal is null)
        {
            throw new UnauthorizedAccessException();
        }

        userInfo.SetPrincipal(principal);
        
        return await next();
    }
}
