using PetFamily.Volunteer.Application.Features.Commands.Volunteer.UpdateMainInfo;
using PetFamily.Volunteer.Contracts.DTOs.Volunteer;

namespace PetFamily.Volunteer.Presentation.Volunteers.Requests;

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
    public UpdateMainVolunteerInfoCommand ToCommand(Guid id)
    {
        var fullName = new FullNameDto(FirstName, LastName, Patronymic);
        
        return new UpdateMainVolunteerInfoCommand(
            id, 
            fullName, 
            Email,
            Description,
            WorkExperience,
            PhoneNumber);
    }
}