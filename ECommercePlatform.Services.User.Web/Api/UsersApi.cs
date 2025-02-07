using ECommercePlatform.Services.User.Application.Commands;
using ECommercePlatform.Services.User.Domain.Models;
using ECommercePlatform.Services.User.Web.Dto;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ECommercePlatform.Services.User.Web.Api;

public static class UsersApi
{
    public static RouteGroupBuilder MapApiEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("api/users");

        api.MapPost("create", CreateUser);
        api.MapDelete("delete/{userId:guid}", DeleteUser);

        return api;
    }

    private static async Task<Ok> CreateUser(UserCreateDto userCreateDto, [AsParameters] UserApiServices services)
    {
        var userCreateCommand = new CreateUserCommand
        {
            MerchantId = userCreateDto.MerchantId,
            FirstName = userCreateDto.FirstName,
            LastName = userCreateDto.LastName,
            Email = userCreateDto.LastName,
            PhoneNumber = userCreateDto.PhoneNumber,
            Address = new Address(userCreateDto.Address.Street,
                userCreateDto.Address.City,
                userCreateDto.Address.State,
                userCreateDto.Address.ZipCode,
                userCreateDto.Address.Country)
        };

        services.Logger.LogInformation("Sending command: {CommandName}", nameof(CreateUserCommand));
        
        var result = await services.Mediator.Send(userCreateCommand);
        
        services.Logger.LogInformation("Command result: {Result}", result);

        return TypedResults.Ok();
    }

    private static async Task<Ok> DeleteUser(Guid userId, [AsParameters] UserApiServices services)
    {
        var userDeleteCommand = new DeleteUserCommand { Id = userId };
        
        services.Logger.LogInformation("Sending command: {CommandName}", nameof(DeleteUserCommand));
        
        var result = await services.Mediator.Send(userDeleteCommand);
        
        services.Logger.LogInformation("Command result: {Result}", result);

        return TypedResults.Ok();
    }
}
