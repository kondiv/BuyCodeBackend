namespace BuyCode.Modules.Auth.Infrastructure.Exceptions;

internal sealed class CouldNotFindRole : Exception
{
    private CouldNotFindRole(string roleName) 
        : base($"Could not find role with name {roleName}")
    {
    }

    public static CouldNotFindRole WithName(string roleName)
    {
        return new CouldNotFindRole(roleName);
    }
}