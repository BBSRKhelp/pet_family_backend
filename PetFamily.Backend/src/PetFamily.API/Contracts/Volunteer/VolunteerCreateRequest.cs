using PetFamily.Application.Commands.Volunteer.Create;
using PetFamily.Application.Dto;

namespace PetFamily.API.Contracts.Volunteer;

public record VolunteerCreateRequest(
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
    public VolunteerCreateCommand ToCommand()
    {
        var fullName = new FullNameDto(FirstName, LastName, Patronymic);
        
        return new VolunteerCreateCommand( 
            fullName,
            Email,
            Description,
            WorkExperience,
            PhoneNumber,
            SocialNetworks,
            Requisites);
    }
}