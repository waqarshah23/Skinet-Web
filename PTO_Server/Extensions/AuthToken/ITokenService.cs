using System.Security.Claims;

namespace PTO_Server.Extensions.AuthToken
{
    public interface ITokenService
   {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
