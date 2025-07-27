using BuyCodeBackend.Auth.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BuyCode.Modules.Auth.Application;

public class AuthenticationService : IAuthenticationService
{
    public Task<IdentityResult> RegisterUserAsync()
    {
        throw new NotImplementedException();
    }
}