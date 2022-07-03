using ArmaForces.Boderator.Core.Infrastructure.Specifications;
using ArmaForces.Boderator.Core.Modsets.Specification;

namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface IDescribedMissionSpecification
{
    IModsetSetMissionSpecification WithModset(IBuildingSpecification<Modset> modsetSpecification);
}