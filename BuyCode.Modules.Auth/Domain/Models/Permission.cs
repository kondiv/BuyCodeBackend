using System.ComponentModel.DataAnnotations;

namespace BuyCodeBackend.Auth.Domain.Models;

internal class Permission
{
    public Guid Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}