namespace PetFamily.File.Application;

public interface IFilesCleanerService
{
    Task Process(CancellationToken cancellationToken = default);
}