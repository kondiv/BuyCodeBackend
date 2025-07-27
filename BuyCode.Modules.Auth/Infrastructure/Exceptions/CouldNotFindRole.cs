namespace BuyCodeBackend.Auth.Infrastructure.Exceptions;

internal class CouldNotFindRole : Exception
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