using Microsoft.AspNetCore.Identity;

namespace SolarWatchAPI.Service.Authentication;

public interface ITokenService
{
    public string CreateToken(IdentityUser user, string role);
}