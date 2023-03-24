using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmaForces.Boderator.BotService.Features.Missions.DTOs;
using ArmaForces.Boderator.BotService.Tests.TestUtilities.TestBases;
using ArmaForces.Boderator.BotService.Tests.TestUtilities.TestFixtures;
using ArmaForces.Boderator.Core.Tests.TestUtilities;
using AutoFixture;
using CSharpFunctionalExtensions;
using Xunit;

namespace ArmaForces.Boderator.BotService.Tests.Features.Missions;

[Trait("Category", "Integration")]
public class GetMissionsControllerTests : ApiTestBase
{
    public GetMissionsControllerTests(TestApiServiceFixture testApi)
        : base(testApi) { }
    
    [Fact]
    public async Task GetMission_MissionExists_ReturnsExistingMission()
    {
        var missionCreateRequest = new MissionCreateRequestDto
        {
            Title = Fixture.Create<string>(),
            Owner = Fixture.Create<string>(),
            Description = Fixture.Create<string>()
        };
            
        var missionCreateResult = await HttpPostAsync<MissionCreateRequestDto, MissionDto>("api/missions", missionCreateRequest);

        var expectedMission = new MissionDto
        {
            Title = missionCreateResult.Value.Title,
            Description = missionCreateRequest.Description,
            Owner = missionCreateRequest.Owner,
            MissionDate = missionCreateResult.Value.MissionDate,
            MissionId = missionCreateResult.Value.MissionId
        };
            
        var result = await HttpGetAsync<MissionDto>($"api/missions/{expectedMission.MissionId}");

        result.ShouldBeSuccess(expectedMission);
    }    
    
    [Fact]
    public async Task GetMissions_SomeMissionsExist_ReturnsAllMissions()
    {
        var createdMissions = await CreateSomeMissions(count: 5);
        var creationResult = createdMissions.Combine();
        creationResult.ShouldBeSuccess();

        var expectedMissions = creationResult.Value
            .Select(x => new MissionDto
            {
                Title = x.createdMission.Title,
                Description = x.createdMission.Description,
                Owner = x.createdMission.Owner,
                MissionDate = x.createdMission.MissionDate,
                MissionId = x.createdMission.MissionId
            })
            .ToList();
            
        var result = await HttpGetAsync<List<MissionDto>>($"api/missions");

        result.ShouldBeSuccess(expectedMissions);
    }

    private async Task<List<Result<(MissionDto createdMission, MissionCreateRequestDto createRequest)>>> CreateSomeMissions(int count = 1)
    {
        return await AsyncEnumerable.Range(0, count)
            .Select(x => new MissionCreateRequestDto
            {
                Title = Fixture.Create<string>(),
                Owner = Fixture.Create<string>(),
                Description = Fixture.Create<string>()
            })
            .SelectAwait(async createRequest => {
                var createResult = await HttpPostAsync<MissionCreateRequestDto, MissionDto>("api/missions", createRequest);
                return (createResult, createRequest);
            })
            .Select(x => x.createResult.IsSuccess
                ? Result.Success((x.createResult.Value, x.createRequest))
                : x.createResult.ConvertFailure<(MissionDto, MissionCreateRequestDto)>())
            .ToListAsync();
    }
}
