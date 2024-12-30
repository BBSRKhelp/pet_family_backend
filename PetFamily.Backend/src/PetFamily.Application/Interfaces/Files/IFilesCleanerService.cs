namespace PetFamily.Application.Interfaces.Files;

public interface IFilesCleanerService
{
    Task Process(CancellationToken cancellationToken = default);
}