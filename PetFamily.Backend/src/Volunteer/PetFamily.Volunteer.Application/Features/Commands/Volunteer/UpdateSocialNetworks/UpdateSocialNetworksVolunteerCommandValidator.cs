using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.Volunteer.Domain.ValueObjects;

namespace PetFamily.Volunteer.Application.Features.Commands.Volunteer.UpdateSocialNetworks;

public class UpdateSocialNetworksVolunteerCommandValidator : AbstractValidator<UpdateSocialNetworksVolunteerCommand>
{
    public UpdateSocialNetworksVolunteerCommandValidator()
    {
        RuleFor(u => u.Id).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));

        RuleForEach(u => u.SocialNetworks)
            .MustBeValueObject(snd => SocialNetwork.Create(snd.Title, snd.Url));
    }
}