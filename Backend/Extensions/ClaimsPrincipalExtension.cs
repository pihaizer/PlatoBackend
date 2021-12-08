using System.Security.Claims;

namespace Backend.Extensions; 

public static class ClaimsPrincipalExtension {
    const string _userIdClaimType = "user_id";
    
    public static string GetFirebaseId(this ClaimsPrincipal principal) {
        return principal.FindFirst(_userIdClaimType)?.Value ?? string.Empty;
    }
}