using FluentValidation;
using PetFamily.Application.Dto;
using PetFamily.Application.Validators;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;

namespace PetFamily.Application.Commands.Volunteer.UpdateSocialNetworks;

public class VolunteerUpdateSocialNetworksCommandValidator : AbstractValidator<VolunteerUpdateSocialNetworksCommand>
{
    public VolunteerUpdateSocialNetworksCommandValidator()
    {
        RuleFor(u => u.Id).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));
        
        RuleForEach(u => u.SocialNetworks)
            .MustBeValueObject(snd => SocialNetwork.Create(snd.Title, snd.Url));
    }
}