namespace PetFamily.Volunteer.Contracts.Requests;

public record UpdateMainInfoVolunteerRequest(
    string FirstName,
    string LastName,
    string? Patronymic,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber
);
// {
//     public UpdateMainVolunteerInfoCommand ToCommand(Guid id)
//     {
//         var fullName = new FullNameDto(FirstName, LastName, Patronymic);
//         
//         return new UpdateMainVolunteerInfoCommand(
//             id, 
//             fullName, 
//             Email,
//             Description,
//             WorkExperience,
//             PhoneNumber);
//     }
// }