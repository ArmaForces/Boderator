using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmaForces.Boderator.Core.Missions;
using ArmaForces.Boderator.Core.Missions.Implementation.Persistence;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Tests.Features.Missions.Helpers;
using ArmaForces.Boderator.Core.Tests.TestUtilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ArmaForces.Boderator.Core.Tests.Features.Missions.Implementation;

public class SignupsCommandServiceIntegrationTests : DatabaseTestBase
{
    private readonly MissionsDbHelper _missionsDbHelper;
    private readonly IMissionCommandService _missionCommandService;
    private readonly ISignupsCommandService _signupsCommandService;

    public SignupsCommandServiceIntegrationTests()
    {
        _missionsDbHelper = ServiceProvider.GetRequiredService<MissionsDbHelper>();
        _missionCommandService = ServiceProvider.GetRequiredService<IMissionCommandService>();
        _signupsCommandService = ServiceProvider.GetRequiredService<ISignupsCommandService>();
    }
    
    [Fact, Trait("Category", "Integration")]
    public async Task CreateSignups_ValidCreateRequest_SignupsCreatedAndReturned()
    {
        var missionCreationResult = await _missionCommandService.CreateMission(MissionsFixture.PrepareCreateRequest());
        
        missionCreationResult.ShouldBeSuccess();
        
        var request = PrepareRequest(missionCreationResult.Value.MissionId);
        
        var expectedSignups = new Signups()
        {
            Status = request.SignupsStatus,
            StartDate = request.StartDate,
            CloseDate = request.CloseDate,
            Teams = request.Teams
        };
        
        await _missionsDbHelper.CreateTestMission();
        
        var result = await _signupsCommandService.CreateSignups(request);
        
        result.ShouldBeSuccess(expectedSignups, opt => opt.Excluding(x => x.SignupsId));
    }
    
    [Fact, Trait("Category", "Integration")]
    public async Task CreateSignups_ValidCreateRequestWithMission_SignupsAndMissionCreatedAndReturned()
    {
        var request = WithMissionCreation(PrepareRequest());
        
        var expectedSignups = new Signups
        {
            Status = request.SignupsStatus,
            StartDate = request.StartDate,
            CloseDate = request.CloseDate,
            Teams = request.Teams
        };
        
        var result = await _signupsCommandService.CreateSignups(request);
        
        result.ShouldBeSuccess(expectedSignups, opt => opt.Excluding(x => x.SignupsId));
    }
    
    // [Fact, Trait("Category", "Integration")]
    // public async Task CreateMission_InvalidModsetNameWithWhitespace_Failure()
    // {
    //     var request = PrepareRequest(modsetName: "Modset with whitespaces");
    //
    //     const string expectedError = $"{nameof(MissionCreateRequest.ModsetName)} cannot contain whitespace characters.";
    //     
    //     var result = await _signupsCommandService.CreateMission(request);
    //     
    //     result.ShouldBeFailure(expectedError);
    // }

    private static SignupsCreateRequest PrepareRequest(long missionId = 1)
    {
        var fixtureSignups = SignupsFixture.CreateTestSignups();
        
        return new SignupsCreateRequest
        {
            MissionId = missionId,
            SignupsStatus = fixtureSignups.Status,
            StartDate = fixtureSignups.StartDate,
            CloseDate = fixtureSignups.CloseDate,
            Teams = fixtureSignups.Teams.ToList()
        };
    }

    private static SignupsCreateRequest WithMissionCreation(SignupsCreateRequest signupsCreateRequest)
    {
        var missionCreateRequest = MissionsFixture.PrepareCreateRequest();

        return signupsCreateRequest with
        {
            MissionId = null,
            MissionCreateRequest = missionCreateRequest
        };
    }
}

