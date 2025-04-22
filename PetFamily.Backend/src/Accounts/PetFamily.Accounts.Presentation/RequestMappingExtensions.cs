using PetFamily.Accounts.Application.Feature.Commands.Login;
using PetFamily.Accounts.Application.Feature.Commands.RegisterUser;
using PetFamily.Accounts.Contracts.Requests;

namespace PetFamily.Accounts.Presentation;

public static class RequestMappingExtensions
{
    public static RegisterUserCommand ToCommand(this RegisterUserRequest request)
    {
        return new RegisterUserCommand(request.Email, request.UserName, request.Password);
    }

    public static LoginCommand ToCommand(this LoginRequest request)
    {
        return new LoginCommand(request.Email, request.Password);
    }
}