using ECommercePlatform.Services.User.Application.Commands;
using ECommercePlatform.Shared.Utils.DataAccess;

using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommercePlatform.Services.User.Application.CommandHandlers;

public class CreateUserCommandHandler(
    IRepository<Domain.Models.User> userRepository, ILogger logger) : IRequestHandler<CreateUserCommand, bool>
{
    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating user");
        
        var newUser = new Domain.Models.User(request.MerchantId,
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.Address);
        
        await userRepository.AddAsync(newUser);
        await userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        
        return true;
    }
}
