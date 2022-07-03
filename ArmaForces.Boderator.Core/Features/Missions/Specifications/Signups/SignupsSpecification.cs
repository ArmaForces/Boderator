using System;
using System.Collections.Generic;
using System.Linq;
using ArmaForces.Boderator.Core.Infrastructure.Specifications;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

namespace ArmaForces.Boderator.Core.Missions.Specifications;

public record SignupsSpecification :
    IExpectCloseDateSpecification,
    IExpectTeamSpecification,
    IExpectAnotherTeamSpecification,
    IBuildingSpecification<Signups>
{
    private SignupsSpecification() { }
    
    private DateTimeOffset StartAt { get; init; }
    
    private DateTimeOffset CloseAt { get; init; }
    
    private IReadOnlyList<IBuildingSpecification<Team>> Teams { get; init; } = new List<IBuildingSpecification<Team>>();

    public static IExpectCloseDateSpecification StartingAt(DateTimeOffset dateTime)
    {
        // TODO: Add date validation
        return new SignupsSpecification
        {
            StartAt = dateTime
        };
    }

    public IExpectTeamSpecification ClosingAt(DateTimeOffset dateTime)
    {
        // TODO: Add date validation
        return this with
        {
            CloseAt = dateTime
        };
    }

    public bool CanAdd(IBuildingSpecification<Team>? team) =>
        team is not null &&
        Teams.All(x => x.Build().Name != team.Build().Name);

    public IExpectAnotherTeamSpecification WithTeam(IBuildingSpecification<Team> team) => AddTeam(team);

    public IExpectAnotherTeamSpecification AndTeam(IBuildingSpecification<Team> team) => AddTeam(team);

    public IBuildingSpecification<Signups> AnoNoMoreTeams()
    {
        return Teams.Any()
            ? this
            : throw new InvalidOperationException("At least one team must be added");
    }

    public Signups Build()
        => new()
        {
            Status = SignupsStatus.Created,
            StartDate = StartAt.DateTime,
            CloseDate = CloseAt.DateTime,
            Teams = Teams.Select(x => x.Build()).ToList()
        };

    private SignupsSpecification AddTeam(IBuildingSpecification<Team> team)
    {
        if (!CanAdd(team))
            throw new ArgumentException(
                "Team cannot be added as it's either null or other team with the same name was already added");

        return this with
        {
            Teams = new List<IBuildingSpecification<Team>>(Teams) {team}
        };
    }
}