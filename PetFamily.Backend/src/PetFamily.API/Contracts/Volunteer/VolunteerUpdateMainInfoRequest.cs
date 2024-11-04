using PetFamily.Application.Commands.Volunteer.UpdateMainInfo;
using PetFamily.Application.Dto;

namespace PetFamily.API.Contracts.Volunteer;

public record VolunteerUpdateMainInfoRequest(
    string FirstName,
    string LastName,
    string? Patronymic,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber
)
{
    public VolunteerUpdateMainInfoCommand ToCommand(Guid id)
    {
        var fullName = new FullNameDto(FirstName, LastName, Patronymic);
        
        return new VolunteerUpdateMainInfoCommand(
            id, 
            fullName, 
            Email,
            Description,
            WorkExperience,
            PhoneNumber);
    }
}