using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Interfaces;

namespace PetFamily.Volunteers.Application.Features.Commands.Volunteer.Create;

public class CreateVolunteerHandler(
    IVolunteersRepository volunteersRepository,
    [FromKeyedServices(UnitOfWorkContext.Volunteers)]
    IUnitOfWork unitOfWork,
    ILogger<CreateVolunteerHandler> logger)
    : ICommandHandler<Guid, CreateVolunteerCommand>
{
    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating Volunteer");

        var volunteer = new Domain.Volunteer();

        var result = await volunteersRepository.AddAsync(volunteer, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("The volunteer was created with the ID: {volunteerId}", result);

        return result;
    }
}