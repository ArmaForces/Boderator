namespace ArmaForces.Boderator.Core.Modsets.Specification;

public record Modset
{
    /// <summary>
    /// Internal constructor to block construction of invalid object.
    /// </summary>
    internal Modset() { }

    public string Name { get; init; } = string.Empty;
}
