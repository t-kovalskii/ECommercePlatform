using ECommercePlatform.Services.User.Application.Commands;
using ECommercePlatform.Shared.Utils.DataAccess;

using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommercePlatform.Services.User.Application.CommandHandlers;

public class DeleteUserCommandHandler(
    IRepository<Domain.Models.User> userRepository, ILogger logger) : IRequestHandler<DeleteUserCommand, bool>
{
    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting user. User Id: {UserId}", request.Id);
        
        var user = await userRepository.GetByIdAsync(request.Id);
        user.SetDeleted();

        await userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        
        return true;
    }
}
