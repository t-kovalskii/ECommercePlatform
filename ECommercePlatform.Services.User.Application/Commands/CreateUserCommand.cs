using MediatR;

namespace ECommercePlatform.Services.User.Application.Commands;

public class CreateUserCommand : IRequest<bool>
{
    public required Guid MerchantId { get; init; }
    
    public required string FirstName { get; init; }
    
    public required string LastName { get; init; }
    
    public required string Email { get; init; }
    
    public required string PhoneNumber { get; init; }
    
    public required string Address { get; init; }
}
