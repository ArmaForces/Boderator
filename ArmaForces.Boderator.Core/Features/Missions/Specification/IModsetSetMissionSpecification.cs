using System;

namespace ArmaForces.Boderator.Core.Missions.Specification;

public interface IModsetSetMissionSpecification
{
    IScheduledMissionSpecification ScheduledAt(DateTimeOffset dateTime);
}