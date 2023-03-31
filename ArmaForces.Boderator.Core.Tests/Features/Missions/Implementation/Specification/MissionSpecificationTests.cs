using System;
using System.Collections.Generic;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Missions.Specifications;
using ArmaForces.Boderator.Core.Modsets.Specification;
using ArmaForces.Boderator.Core.Users;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace ArmaForces.Boderator.Core.Tests.Features.Missions.Implementation.Specification;

[Trait("Category", "Unit")]
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
                Teams = new List<Team>
                {
                    new()
                    {
                        Name = "Alpha",
                        Slots = new List<Slot>
                        {
                            new()
                            {
                                Name = "SL"
                            }
                        }
                    },
                    new()
                    {
                        Name = "Bravo",
                        Slots = new List<Slot>
                        {
                            new()
                            {
                                Name = "SL"
                            }
                        }
                    }
                }
            }
        };

        var specification = MissionSpecification
            .OwnedBy(new User(expectedMission.Owner))
            .Titled(expectedMission.Title)
            .WithDescription(expectedMission.Description)
            .WithModset(ModsetSpecification
                .Named(expectedMission.ModsetName))
            .ScheduledAt(expectedMission.MissionDate.Value)
            .WithSignups(SignupsSpecification
                .StartingAt(expectedMission.Signups.StartDate.Value)
                .ClosingAt(expectedMission.Signups.CloseDate.Value)
                .WithTeam(TeamSpecification
                    .Named("Alpha")
                    .WithoutVehicle()
                    .WithSlot(SlotSpecification
                        .UnoccupiedWithoutVehicleNamed("SL"))
                    .AndNoMoreSlots())
                .AndTeam(TeamSpecification
                    .Named("Bravo")
                    .WithoutVehicle()
                    .WithSlot(SlotSpecification
                        .UnoccupiedWithoutVehicleNamed("SL"))
                    .AndNoMoreSlots())
                .AnoNoMoreTeams());

        var mission = specification.Build();

        mission.Should().BeEquivalentTo(expectedMission);
    } 
}
