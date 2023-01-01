using System.Collections.Generic;
using System.Threading.Tasks;
using ArmaForces.Boderator.BotService.Features.Missions.DTOs;
using ArmaForces.Boderator.BotService.Tests.TestUtilities.TestBases;
using ArmaForces.Boderator.BotService.Tests.TestUtilities.TestFixtures;
using ArmaForces.Boderator.Core.Tests.TestUtilities;
using Xunit;

namespace ArmaForces.Boderator.BotService.Tests.Features.Missions
{
    public class SignupsControllerTests : ApiTestBase
    {
        public SignupsControllerTests(TestApiServiceFixture testApi)
            : base(testApi) { }

        [Theory, ClassData(typeof(CreateSignupsInvalidRequestTestData)), Trait("Category", "Integration")]
        public async Task CreateSignups_InvalidRequest_ReturnsBadRequest(
            SignupsCreateRequestDto signupsCreateRequestDto)
        {
            var result =
                await HttpPostAsync<SignupsCreateRequestDto, SignupsDto>("api/signups", signupsCreateRequestDto);

            result.ShouldBeFailure();
        }

        private class CreateSignupsInvalidRequestTestData : TheoryData<SignupsCreateRequestDto>
        {
            public CreateSignupsInvalidRequestTestData()
            {
                var testCases = new List<SignupsCreateRequestDto>
                {
                    new()
                    {
                        Teams = new List<TeamDto>()
                    },
                    new()
                    {
                        Mission = new MissionCreateRequestDto(),
                        Teams = new List<TeamDto>()
                    },
                    new()
                    {
                        MissionId = 0,
                        Teams = new List<TeamDto>()
                    },
                    new()
                    {
                        MissionId = 0,
                        Mission = new MissionCreateRequestDto(),
                        Teams = new List<TeamDto>()
                    }
                };

                foreach (var testCase in testCases) Add(testCase);
            }
        }
    }
}
