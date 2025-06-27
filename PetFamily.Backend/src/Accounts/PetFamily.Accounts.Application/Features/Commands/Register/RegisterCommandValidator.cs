using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Application.Features.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(command => command.UserName)
            .NotEmpty()
            .MaximumLength(Constants.MAX_NAME_LENGTH)
            .WithError(Errors.General.MaxLengthExceeded("UserName"));
        
        RuleFor(r => r.FullName)
            .MustBeValueObject(fnd => FullName.Create(fnd.FirstName, fnd.LastName, fnd.Patronymic));

        RuleFor(r => r.Email).MustBeValueObject(Email.Create);
        
        RuleForEach(c => c.SocialNetworks)
            .MustBeValueObject(snd => SocialNetwork.Create(snd.Title, snd.Url));
    }
}