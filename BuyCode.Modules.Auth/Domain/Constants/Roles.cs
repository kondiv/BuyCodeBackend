namespace BuyCode.Modules.Auth.Domain.Constants;

internal static class Roles
{
    public const string Admin = "Admin";
    public const string User = "User";
    public const string Developer = "Developer";

    public static IReadOnlyCollection<string> All() => [Admin, User, Developer];
}