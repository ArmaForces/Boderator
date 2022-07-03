using System;
using ArmaForces.Boderator.Core.Infrastructure.Specifications;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

namespace ArmaForces.Boderator.Core.Missions.Specifications;

public record SignupsSpecification :
    IStartingSignupsSpecification,
    IBuildingSpecification<Signups>
{
    private SignupsSpecification() { }
    
    private DateTimeOffset StartAt { get; init; }
    
    private DateTimeOffset CloseAt { get; init; }

    public static IStartingSignupsSpecification StartingAt(DateTimeOffset dateTime)
    {
        return new SignupsSpecification()
        {
            StartAt = dateTime
        };
    }

    public IBuildingSpecification<Signups> ClosingAt(DateTimeOffset dateTime)
    {
        return this with
        {
            CloseAt = dateTime
        };
    }

    public Signups Build()
        => new()
        {
            Status = SignupsStatus.Created,
            StartDate = StartAt.DateTime,
            CloseDate = CloseAt.DateTime
        };
}