using MediatR;

namespace ECommercePlatform.Services.User.Application.Commands;

public class UpdateUserPasswordCommand : IRequest<bool>
{
    public required Guid Id { get; init; }
    
    public required string Password { get; init; }
    
    public required string NewPassword { get; init; }
}
