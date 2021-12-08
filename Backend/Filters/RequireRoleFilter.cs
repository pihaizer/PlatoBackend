using System.Security.Claims;

using Backend.Extensions;
using Backend.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class RequireAdminAttribute : TypeFilterAttribute {
    public RequireAdminAttribute() : base(
        typeof(RequireRoleFilter)) {
        Arguments = new object[] { new[] { ClaimRole.Admin, ClaimRole.SuperUser } };
    }
}


public class RequireSuperuserAttribute : TypeFilterAttribute {
    public RequireSuperuserAttribute() : base(
        typeof(RequireRoleFilter)) {
        Arguments = new object[] { new[] { ClaimRole.SuperUser } };
    }
}

public class RequireRoleFilter : IAuthorizationFilter {
    readonly string[] _possibleRoles;

    public RequireRoleFilter(string[] possibleRoles) {
        _possibleRoles = possibleRoles;
    }

    public void OnAuthorization(AuthorizationFilterContext context) {
        Console.WriteLine(context.HttpContext.User.GetFirebaseId());
        Console.WriteLine(string.Join('\n', context.HttpContext.User.Claims));
        bool hasClaim = context.HttpContext.User.Claims
            .Any(c => c.Type == ClaimTypes.Role && _possibleRoles.Contains(c.Value));
        if (!hasClaim) {
            context.Result = new ForbidResult();
        }
    }
}