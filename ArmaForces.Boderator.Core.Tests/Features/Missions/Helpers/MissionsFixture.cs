using System;
using ArmaForces.Boderator.Core.Missions.Models;

namespace ArmaForces.Boderator.Core.Tests.Features.Missions.Helpers;

internal static class MissionsFixture
{
    public static MissionCreateRequest PrepareCreateRequest(
        string? modsetName = null)
    {
        var fixtureMission = CreateTestMission(modsetName);
        
        return new MissionCreateRequest
        {
            Title = fixtureMission.Title,
            Description = fixtureMission.Description,
            Owner = fixtureMission.Owner,
            MissionDate = fixtureMission.MissionDate,
            ModsetName = fixtureMission.ModsetName
        };
    }
    
    public static Mission CreateTestMission(
        string? modsetName = null)
    {
        return new Mission
        {
            Title = "Test mission",
            Owner = "Tester",
            MissionDate = DateTime.Today.AddHours(20),
            ModsetName = modsetName ?? "Test-modset",
            Description = "Test description"
        };
    }
}
