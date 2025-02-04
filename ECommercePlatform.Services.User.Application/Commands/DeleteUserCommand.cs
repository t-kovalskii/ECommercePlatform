using MediatR;

namespace ECommercePlatform.Services.User.Application.Commands;

public class DeleteUserCommand : IRequest<bool>
{
    public required Guid Id { get; init; }
}
