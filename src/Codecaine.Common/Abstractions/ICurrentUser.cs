namespace Codecaine.Common.Abstractions
{
    /// <summary>
    /// Represents the current user in the application.
    /// </summary>
    public interface ICurrentUser
    {
        string UserId { get; }
        string UserName { get; }
    }
}
