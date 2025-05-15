namespace PetFamily.Files.Application;

public interface IFilesCleanerService
{
    Task Process(CancellationToken cancellationToken = default);
}