using PetFamily.Application.DTOs;
using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Application.VolunteerAggregate.Commands.Volunteer.Create;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record CreateVolunteerRequest(
    string FirstName,
    string LastName,
    string? Patronymic,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto>? SocialNetworks,
    IEnumerable<RequisiteDto>? Requisites)
{
    public CreateVolunteerCommand ToCommand()
    {
        var fullName = new FullNameDto(FirstName, LastName, Patronymic);
        
        return new CreateVolunteerCommand( 
            fullName,
            Email,
            Description,
            WorkExperience,
            PhoneNumber,
            SocialNetworks,
            Requisites);
    }
}