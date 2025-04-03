using PetFamily.Volunteer.Contracts.DTOs;
using PetFamily.Volunteer.Contracts.DTOs.Volunteer;

namespace PetFamily.Volunteer.Contracts.Requests;

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
    /*public CreateVolunteerCommand ToCommand()
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
    }*/
}