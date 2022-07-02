using System;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Users;

namespace ArmaForces.Boderator.Core.Missions.Specification;

public record SignupsSpecification :
    IStartingSignupsSpecification,
    IBuildingSignupsSpecification
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

    public IBuildingSignupsSpecification ClosingAt(DateTimeOffset dateTime)
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

public interface IStartingSignupsSpecification
{
    IBuildingSignupsSpecification ClosingAt(DateTimeOffset dateTime);
}