using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ArmaForces.Boderator.BotService.Features.Missions.DTOs;
using ArmaForces.Boderator.BotService.Tests.LegacyImportTest.MissionsApi;
using ArmaForces.Boderator.BotService.Tests.TestUtilities.TestBases;
using ArmaForces.Boderator.BotService.Tests.TestUtilities.TestFixtures;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Tests.TestUtilities;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Xunit;

namespace ArmaForces.Boderator.BotService.Tests.LegacyImportTest;

[Trait("Category", "Functional")]
public class LegacySignupsImportTest : ApiTestBase
{
    private const string MissionsResource = @"https://boderator.armaforces.com/api/missions?includeArchive=true";
    
    public LegacySignupsImportTest(TestApiServiceFixture testApi) : base(testApi)
    {
    }

    [Fact]
    public async Task ImportLegacySignups_OnlyMissionData_Imported()
    {
        var legacyMissionsResult = await HttpGetAsync<IEnumerable<WebMission>>(MissionsResource);

        var createdMissions = await legacyMissionsResult
            .Map(missions => missions
                .OrderBy(x => x.Date)
                .Select(
                    mission => new SignupsCreateRequestDto
                    {
                        CloseDate = mission.CloseDate,
                        SignupsStatus = mission.Archive ? SignupsStatus.Closed : SignupsStatus.Open,
                        StartDate = mission.CloseDate, // TODO: Try to figure signups start date possibly?
                        Mission = new MissionCreateRequestDto
                        {
                            Title = mission.Title,
                            ModsetName = mission.Modlist.Replace(" ", "-"),
                            Description = mission.Description,
                            MissionDate = mission.Date,
                            Owner = "AF"
                        }
                    }))
            .Bind(async newMissions =>
            {
                var creationResults = new List<Result<SignupsDto>>();
                
                foreach (var signupsCreateRequestDto in newMissions)
                {
                    var result =
                        await HttpPostAsync<SignupsCreateRequestDto, SignupsDto>("api/signups",
                            signupsCreateRequestDto);
                    
                    creationResults.Add(result);
                }

                return creationResults.Combine();
            });

        var missions = await HttpGetAsync<List<MissionDto>>($"api/missions");
        
    }
}