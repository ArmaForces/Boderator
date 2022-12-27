using System;
using System.Collections.Generic;

namespace ArmaForces.Boderator.Core.Missions.Models;

public record SignupsCreateRequest
{
    public long? MissionId { get; init; }
    
    public MissionCreateRequest? MissionCreateRequest { get; init; }

    public SignupsStatus SignupsStatus { get; init; } = SignupsStatus.Created;
    
    public DateTime? StartDate { get; init; }
    
    public DateTime? CloseDate { get; init; }

    public List<Team> Teams { get; init; } = new();
}
