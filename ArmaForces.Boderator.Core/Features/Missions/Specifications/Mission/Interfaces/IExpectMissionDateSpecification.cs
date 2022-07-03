using System;

namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface IExpectMissionDateSpecification
{
    IExpectMissionSignupsSpecification ScheduledAt(DateTimeOffset dateTime);
}