﻿using ArmaForces.Boderator.Core.Infrastructure.Specifications;
using ArmaForces.Boderator.Core.Missions.Models;

namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface IScheduledMissionSpecification
{
    IBuildingSpecification<Mission> WithSignups(IBuildingSpecification<Signups> signupsSpecification);
}