using BuyCode.Modules.Auth.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BuyCode.Modules.Auth.Application;

public sealed class AuthenticationService : IAuthenticationService
{
    public Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistrationDto)
    {
        throw new NotImplementedException();
    }
}