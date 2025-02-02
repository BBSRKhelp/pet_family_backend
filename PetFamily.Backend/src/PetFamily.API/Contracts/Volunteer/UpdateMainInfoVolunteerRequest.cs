using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Application.VolunteerAggregate.Commands.UpdateMainInfo;

namespace PetFamily.API.Contracts.Volunteer;

public record UpdateMainInfoVolunteerRequest(
    string FirstName,
    string LastName,
    string? Patronymic,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber
)
{
    public UpdateMainInfoVolunteerCommand ToCommand(Guid id)
    {
        var fullName = new FullNameDto(FirstName, LastName, Patronymic);
        
        return new UpdateMainInfoVolunteerCommand(
            id, 
            fullName, 
            Email,
            Description,
            WorkExperience,
            PhoneNumber);
    }
}