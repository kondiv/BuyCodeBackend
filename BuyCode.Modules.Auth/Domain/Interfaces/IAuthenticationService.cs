using Microsoft.AspNetCore.Identity;

namespace BuyCode.Modules.Auth.Domain.Interfaces;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUserAsync();
}