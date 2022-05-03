using System.Threading.Tasks;
using ArmaForces.Boderator.Core.Missions.Implementation.Persistence;
using ArmaForces.Boderator.Core.Missions.Models;
using FluentAssertions;

namespace ArmaForces.Boderator.Core.Tests.Features.Missions.Helpers;

internal class SignupsDbHelper
{
    private readonly MissionsDbHelper _missionsDbHelper;
    private readonly MissionContext _missionContext;

    public SignupsDbHelper(
        MissionsDbHelper missionsDbHelper,
        MissionContext missionContext)
    {
        _missionsDbHelper = missionsDbHelper;
        _missionContext = missionContext;
    }

    public async Task<Signups> CreateTestSignups(Mission mission)
    {
        var signups = SignupsFixture.CreateTestSignups();

        var updatedMission = mission with
        {
            Signups = signups
        };

        _missionContext.Attach(updatedMission);
        _missionContext.Entry(updatedMission).Reference(x => x.Signups).IsModified = true;

        await _missionContext.SaveChangesAsync();
        _missionContext.ChangeTracker.Clear();
        
        var addedEntry = await _missionContext.Signups.FindAsync(signups.SignupsId);

        addedEntry.Should().NotBeNull();
        
        return addedEntry!;
    }

    public async Task<Signups> CreateTestSignups()
    {
        var mission = await _missionsDbHelper.CreateTestMission();
        return await CreateTestSignups(mission);
    }
}