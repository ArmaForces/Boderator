using System;
using System.Collections.Generic;

namespace ArmaForces.Boderator.Core.Missions.Models;

public record Signups
{
    public long SignupsId { get; init; }

    public SignupsStatus Status { get; init; } = SignupsStatus.Created;
        
    public DateTime StartDate { get; init; }
        
    public DateTime CloseDate { get; init; }
        
    public IReadOnlyList<Team> Teams { get; init; } = new List<Team>();
}