using Microsoft.AspNetCore.Identity;

namespace BuyCodeBackend.Auth.Domain.Interfaces;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUserAsync();
}