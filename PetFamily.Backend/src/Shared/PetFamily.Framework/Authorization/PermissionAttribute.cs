using Microsoft.AspNetCore.Authorization;

namespace PetFamily.Framework.Authorization;

//Одновременно и аттрибут и требование
public class PermissionAttribute : AuthorizeAttribute, IAuthorizationRequirement
{
    public PermissionAttribute(string code)
        : base(code)
    {
        Code = code;
    }
    
    public string Code { get; set; }
}