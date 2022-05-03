using System;
using System.Collections.Generic;
using ArmaForces.Boderator.Core.Missions.Models;

namespace ArmaForces.Boderator.Core.Tests.Features.Missions.Helpers;

internal static class SignupsFixture
{
    public static Signups CreateTestSignups()
    {
        return new Signups
        {
            Status = SignupsStatus.Open,
            StartDate = DateTime.Now,
            CloseDate = DateTime.Now.AddHours(1),
            Teams = new List<Team>()
        };
    }
}
