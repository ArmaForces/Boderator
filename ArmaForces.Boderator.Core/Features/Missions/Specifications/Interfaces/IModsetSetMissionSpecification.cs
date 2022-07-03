using System;

namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface IModsetSetMissionSpecification
{
    IScheduledMissionSpecification ScheduledAt(DateTimeOffset dateTime);
}