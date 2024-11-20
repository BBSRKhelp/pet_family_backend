namespace PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;

public record PetPhotosShell
{
    //ef core
    private PetPhotosShell()
    {
    }

    public PetPhotosShell(IEnumerable<PetPhoto> petPhotos)
    {
        PetPhotos = petPhotos.ToList();
    }

    public IReadOnlyList<PetPhoto> PetPhotos { get; } = null!;
}