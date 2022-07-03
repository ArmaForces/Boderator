using System;

namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface IExpectCloseDateSpecification
{
    IExpectTeamSpecification ClosingAt(DateTimeOffset dateTime);
}
