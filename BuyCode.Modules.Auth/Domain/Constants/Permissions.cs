namespace BuyCodeBackend.Auth.Domain.Constants;

internal static class Permissions
{
    public const string ViewProjects = "ViewProjects";
    public const string PublishProjects = "PublishProjects";
    public const string PublishTasks = "PublishTasks";
    public const string HandleTasks = "HandleTasks";
    public const string MessageDevelopers = "MessageDevelopers";
    public const string MessageEmployers = "MessageEmployers";
    public const string CommentDevelopersWork = "CommentDevelopersWork";
    public const string CommentEmployersWork = "CommentEmployersWork";

    public static IReadOnlyCollection<string> All() =>
    [
        ViewProjects,
        PublishProjects,
        PublishTasks,
        HandleTasks,
        MessageDevelopers,
        MessageEmployers,
        CommentDevelopersWork,
        CommentEmployersWork,
    ];
}