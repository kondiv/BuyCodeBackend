using Microsoft.AspNetCore.Identity;

namespace BuyCodeBackend.Auth.Domain.Models;

internal class Role : IdentityRole<Guid>
{
    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}