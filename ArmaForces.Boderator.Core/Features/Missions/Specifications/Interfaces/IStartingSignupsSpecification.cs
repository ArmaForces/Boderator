using System;
using ArmaForces.Boderator.Core.Infrastructure.Specifications;
using ArmaForces.Boderator.Core.Missions.Models;

namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface IStartingSignupsSpecification
{
    IBuildingSpecification<Signups> ClosingAt(DateTimeOffset dateTime);
}
