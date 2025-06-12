using Microsoft.AspNetCore.Identity;

namespace PetFamily.Accounts.Domain.DataModels;

public class User : IdentityUser<Guid>
{
    private List<Role> _roles = [];

    private User() { }
    
    public IReadOnlyList<Role> Roles => _roles;

    public static User CreateUser(string userName, string email)
    {
        return new User
        {
            UserName = userName,
            Email = email
        };
    }
    
    public static User CreateAdmin(string userName, string email, Role role)
    {
        return new User
        {
            UserName = userName,
            Email = email,
            _roles = [role]
        };
    }

    public void SetRoles(IEnumerable<Role> roles)
    {
        _roles = roles.ToList();
    }
}