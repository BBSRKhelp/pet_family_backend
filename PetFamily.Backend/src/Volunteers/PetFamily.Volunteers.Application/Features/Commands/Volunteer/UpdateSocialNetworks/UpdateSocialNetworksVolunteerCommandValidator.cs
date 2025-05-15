using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Features.Commands.Volunteer.UpdateSocialNetworks;

public class UpdateSocialNetworksVolunteerCommandValidator : AbstractValidator<UpdateSocialNetworksVolunteerCommand>
{
    public UpdateSocialNetworksVolunteerCommandValidator()
    {
        RuleFor(u => u.Id).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));

        RuleForEach(u => u.SocialNetworks)
            .MustBeValueObject(snd => SocialNetwork.Create(snd.Title, snd.Url));
    }
}