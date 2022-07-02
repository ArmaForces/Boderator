namespace ArmaForces.Boderator.Core.Users;

public record User
{
    /// <summary>
    /// Internal constructor to block construction of invalid object.
    /// </summary>
    internal User() { }

    internal User(string userName)
    {
        Name = userName;
    }

    public string Name { get; init; } = string.Empty;

    public override string ToString() => Name;
}
