using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BuyCodeBackend.Auth.Domain.Models;

internal class User : IdentityUser<Guid>
{
    [MaxLength(40)]
    public string FullName { get; set; } = null!;
    [MaxLength(256)]
    public string? AvatarUrl { get; set; }
}