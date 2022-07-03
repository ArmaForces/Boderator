using System.Collections.Generic;

namespace ArmaForces.Boderator.Core.Missions.Models;

public record Team
{
    public string Name { get; init; } = string.Empty;

    public string? Vehicle { get; init; }

    public IReadOnlyList<Slot> Slots { get; init; } = new List<Slot>();
}