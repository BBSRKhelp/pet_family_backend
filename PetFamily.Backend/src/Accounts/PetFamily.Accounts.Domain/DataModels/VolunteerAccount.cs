using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain.DataModels;

public class VolunteerAccount
{
    public const string VOLUNTEER = nameof(VOLUNTEER);
    private IReadOnlyList<Certificate> _certificates = [];

    //ef core
    private VolunteerAccount()
    {
    }

    public VolunteerAccount(
        WorkExperience workExperience,
        IReadOnlyList<Requisite> requisites,
        IReadOnlyList<Certificate> certificates,
        User user)
    {
        Id = Guid.NewGuid();
        WorkExperience = workExperience;
        Requisites = requisites;
        _certificates = certificates;
        User = user;
    }

    public Guid Id { get; init; }
    public WorkExperience WorkExperience { get; init; } = null!;
    public IReadOnlyList<Requisite> Requisites { get; private set; } = null!;

    public IReadOnlyList<Certificate> Certificates =>
        _certificates; //TODO СПРОСИТЬ через private set или через выделение нового private поля 

    public Guid UserId { get; init; }
    public User User { get; init; } = null!;

    public void SetCertificates(IEnumerable<Certificate> certificates)
        => _certificates = certificates.ToList();

    public void SetRequisites(IEnumerable<Requisite> requisites)
        => Requisites = requisites.ToList();
}