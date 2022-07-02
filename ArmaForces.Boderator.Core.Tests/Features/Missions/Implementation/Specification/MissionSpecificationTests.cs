using System;
using System.Collections.Generic;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Missions.Specification;
using ArmaForces.Boderator.Core.Modsets.Specification;
using ArmaForces.Boderator.Core.Users;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace ArmaForces.Boderator.Core.Tests.Features.Missions.Implementation.Specification;

public class MissionSpecificationTests
{
    private readonly Fixture _fixture = new();
    
    [Fact]
    public void CreateSpecification_FullSpecification_ValidMissionBuilt()
    {
        var expectedOwner = new User
        {
            Name = _fixture.Create<string>()
        };
        
        var expectedMission = new Mission
        {
            Title = _fixture.Create<string>(),
            Description = _fixture.Create<string>(),
            MissionDate = _fixture.Create<DateTime>(),
            ModsetName = _fixture.Create<string>(),
            Owner = expectedOwner.Name,
            Signups = new Signups
            {
                StartDate = _fixture.Create<DateTime>(),
                CloseDate = _fixture.Create<DateTime>(),
                Status = SignupsStatus.Created,
                Teams = new List<Team>()
            }
        };

        var specification = MissionSpecification
            .OwnedBy(new User(expectedMission.Owner))
            .WithTitle("Test title")
            .WithDescription("Test description")
            .WithModset(ModsetSpecification
                .ByName("modset name"))
            .ScheduledAt(DateTimeOffset.Now)
            .WithSignups(SignupsSpecification
                .StartingAt(DateTimeOffset.Now.AddDays(1))
                .ClosingAt(DateTimeOffset.Now.AddDays(1)));

        var mission = specification.Build();

        mission.Should().BeEquivalentTo(expectedMission);
    } 
}
