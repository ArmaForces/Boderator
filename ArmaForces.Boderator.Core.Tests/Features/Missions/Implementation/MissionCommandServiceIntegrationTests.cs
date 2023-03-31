using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmaForces.Boderator.Core.Missions;
using ArmaForces.Boderator.Core.Missions.Implementation.Persistence;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Tests.Features.Missions.Helpers;
using ArmaForces.Boderator.Core.Tests.TestUtilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ArmaForces.Boderator.Core.Tests.Features.Missions.Implementation;

public class MissionCommandServiceIntegrationTests : DatabaseTestBase
{
    private readonly MissionsDbHelper _missionsDbHelper;
    private readonly IMissionQueryService _missionQueryService;
    private readonly IMissionCommandService _missionCommandService;

    public MissionCommandServiceIntegrationTests()
    {
        _missionsDbHelper = ServiceProvider.GetRequiredService<MissionsDbHelper>();
        _missionQueryService = ServiceProvider.GetRequiredService<IMissionQueryService>();
        _missionCommandService = ServiceProvider.GetRequiredService<IMissionCommandService>();
    }
    
    [Fact, Trait("Category", "Integration")]
    public async Task CreateMission_ValidCreateRequest_MissionCreatedAndReturned()
    {
        var request = MissionsFixture.PrepareCreateRequest();

        var expectedMission = new Mission
        {
            Title = request.Title,
            Description = request.Description,
            Owner = request.Owner,
            MissionDate = request.MissionDate,
            ModsetName = request.ModsetName
        };
        
        var result = await _missionCommandService.CreateMission(request);
        
        result.ShouldBeSuccess(expectedMission, opt => opt.Excluding(x => x.MissionId));
    }
    
    [Fact, Trait("Category", "Integration")]
    public async Task CreateMission_InvalidModsetNameWithWhitespace_Failure()
    {
        var request = MissionsFixture.PrepareCreateRequest(modsetName: "Modset with whitespaces");

        const string expectedError = $"{nameof(MissionCreateRequest.ModsetName)} cannot contain whitespace characters.";
        
        var result = await _missionCommandService.CreateMission(request);
        
        result.ShouldBeFailure(expectedError);
    }
}
