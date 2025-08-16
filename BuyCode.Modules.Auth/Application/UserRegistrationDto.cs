using Kernel.Values;

namespace BuyCode.Modules.Auth.Application;

public record UserRegistrationDto(Email Email, string Password);