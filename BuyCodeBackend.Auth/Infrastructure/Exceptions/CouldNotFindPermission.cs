namespace BuyCodeBackend.Auth.Infrastructure.Exceptions;

public class CouldNotFindPermission : Exception
{
    private CouldNotFindPermission(string permissionName) 
        : base($"Could not find permission with name {permissionName}")
    {
    }

    public static CouldNotFindPermission WithName(string permissionName)
    {
        return new CouldNotFindPermission(permissionName);
    }
}