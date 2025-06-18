using PetFamily.Accounts.Application.Feature.Commands.Login;
using PetFamily.Accounts.Application.Feature.Commands.RegisterUser;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Core.DTOs;

namespace PetFamily.Accounts.Presentation;

public static class RequestMappingExtensions
{
    public static RegisterUserCommand ToCommand(this RegisterUserRequest request)
    {
        var fullName = new FullNameDto(request.FirstName, request.LastName,  request.Patronymic);
        
        return new RegisterUserCommand(
            request.UserName, 
            fullName,
            request.Email, 
            request.SocialNetworks, 
            request.Password);
    }

    public static LoginCommand ToCommand(this LoginRequest request)
    {
        return new LoginCommand(request.Email, request.Password);
    }
}