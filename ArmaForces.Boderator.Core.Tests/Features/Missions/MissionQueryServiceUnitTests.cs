﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmaForces.Boderator.Core.Missions.Implementation;
using ArmaForces.Boderator.Core.Missions.Implementation.Persistence;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Tests.TestUtilities;
using AutoFixture;
using Moq;
using Xunit;

namespace ArmaForces.Boderator.Core.Tests.Features.Missions;

public class MissionQueryServiceUnitTests
{
    private readonly Fixture _fixture = new();
    
    [Fact, Trait("Category", "Unit")]
    public async Task GetMissions_RepositoryEmpty_ReturnsEmptyList()
    {
        var missionQueryRepositoryMock = new Mock<IMissionRepository>();
        missionQueryRepositoryMock
            .Setup(x => x.GetMissions())
            .Returns(Task.FromResult(new List<Mission>()));
        
        var missionQueryService = new MissionQueryService(missionQueryRepositoryMock.Object);

        var result = await missionQueryService.GetMissions();
        
        result.ShouldBeSuccess(new List<Mission>());
    }
    
    [Fact, Trait("Category", "Unit")]
    public async Task GetMissions_RepositoryNotEmpty_ReturnsExpectedMissions()
    {
        var missionsInRepository = _fixture.CreateMany<Mission>(5).ToList();
            
        var missionQueryRepositoryMock = new Mock<IMissionRepository>();
        missionQueryRepositoryMock
            .Setup(x => x.GetMissions())
            .Returns(Task.FromResult(missionsInRepository));
        
        var missionQueryService = new MissionQueryService(missionQueryRepositoryMock.Object);

        var result = await missionQueryService.GetMissions();
        
        result.ShouldBeSuccess(missionsInRepository);
    }
}