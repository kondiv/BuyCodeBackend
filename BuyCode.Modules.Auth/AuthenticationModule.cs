using BuyCode.Modules.Auth.Infrastructure.DBContexts;
using BuyCode.Modules.Auth.Infrastructure.Seeders;
using BuyCodeBackend.Auth.Domain.Models;
using Kernel.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuyCode.Modules.Auth;

public static class AuthenticationModule
{
    public static IServiceCollection AddAuthenticationModule(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AuthContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddIdentity<User, Role>().AddEntityFrameworkStores<AuthContext>();
        
        services.AddScoped<IDataSeeder, RolePermissionSeeder>();

        return services;
    }
}