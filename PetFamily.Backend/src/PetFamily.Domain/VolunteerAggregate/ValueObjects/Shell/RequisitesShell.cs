namespace PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;

public record RequisitesShell
{
    //ef core
    private RequisitesShell()
    {
    }

    public RequisitesShell(IEnumerable<Requisite> requisites)
    {
        Requisites = requisites.ToList();
    }

    public IReadOnlyList<Requisite> Requisites { get; } = null!;
}