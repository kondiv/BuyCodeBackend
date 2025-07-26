using BuyCodeBackend.Auth.Domain.Constants;
using BuyCodeBackend.Auth.Domain.Models;
using BuyCodeBackend.Auth.Infrastructure.DBContexts;
using BuyCodeBackend.Auth.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BuyCodeBackend.Auth.Infrastructure.Seeders;

internal class RolePermissionSeeder
{
    private readonly AuthContext _context;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<RolePermissionSeeder> _logger;

    private readonly Dictionary<string, List<string>> _rolePermissions = new Dictionary<string, List<string>>()
    {
        [Roles.Developer] = [
            Permissions.ViewProjects,
            Permissions.CommentEmployersWork,
            Permissions.HandleTasks,
            Permissions.MessageEmployers,
            Permissions.PublishProjects,
        ],
        
        [Roles.User] = [
            Permissions.ViewProjects,
            Permissions.CommentEmployersWork,
            Permissions.MessageDevelopers,
            Permissions.CommentDevelopersWork,
            Permissions.PublishTasks
        ],
        
        [Roles.Admin] = Permissions.All().ToList()
    };

    public RolePermissionSeeder(AuthContext context, RoleManager<Role> roleManager,
        ILogger<RolePermissionSeeder> logger)
    {
        _context = context;
        _roleManager = roleManager;
        _logger = logger;
    }
    
    public async Task SeedAsync()
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await SeedRoles();
            await SeedPermissions();
            await SeedRolePermissions();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        
        await transaction.CommitAsync();
    }

    private async Task SeedRoles()
    {
        foreach (var role in Roles.All())
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new Role() { Name = role });
                _logger.LogInformation($"Created role {role}");
            }
        }
    }

    private async Task SeedPermissions()
    {
        foreach (var permission in Permissions.All())
        {
            if (!await _context.Permissions.AnyAsync(p => p.Name == permission))
            {
                await _context.Permissions.AddAsync(new Permission { Name = permission });
                _logger.LogInformation($"Created permission {permission}");
            }
        }
        
        await _context.SaveChangesAsync();
    }

    private async Task SeedRolePermissions()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        var permissions = await _context.Permissions.ToListAsync();

        foreach (var rolePermission in _rolePermissions)
        {
            var role = roles.FirstOrDefault(r => r.Name == rolePermission.Key);

            if (role == null)
            {
                throw CouldNotFindRole.WithName(rolePermission.Key);
            }

            foreach (var permission in rolePermission.Value)
            {
                var actualPermission = permissions.FirstOrDefault(p => p.Name == permission);

                if (actualPermission == null)
                {
                    throw CouldNotFindPermission.WithName(permission);
                }
                
                var exist = await _context.RolePermissions.AnyAsync(rp =>
                    rp.RoleId == role.Id && rp.PermissionId == actualPermission.Id);

                if (!exist)
                {
                    await _context.RolePermissions.AddAsync(new RolePermission
                        { RoleId = role.Id, PermissionId = actualPermission.Id });
                    _logger.LogInformation($"Created role permission {actualPermission.Id}");
                }
            }
        }

        await _context.SaveChangesAsync();
    }
}