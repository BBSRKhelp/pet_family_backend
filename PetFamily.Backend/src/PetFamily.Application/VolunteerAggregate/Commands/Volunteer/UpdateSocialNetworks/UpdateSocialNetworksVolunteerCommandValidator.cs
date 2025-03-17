using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Core.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.VolunteerAggregate.Commands.Volunteer.UpdateSocialNetworks;

public class UpdateSocialNetworksVolunteerCommandValidator : AbstractValidator<UpdateSocialNetworksVolunteerCommand>
{
    public UpdateSocialNetworksVolunteerCommandValidator()
    {
        RuleFor(u => u.Id).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));

        RuleForEach(u => u.SocialNetworks)
            .MustBeValueObject(snd => SocialNetwork.Create(snd.Title, snd.Url));
    }
}