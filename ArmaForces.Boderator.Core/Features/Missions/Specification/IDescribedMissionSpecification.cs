using ArmaForces.Boderator.Core.Modsets.Specification;

namespace ArmaForces.Boderator.Core.Missions.Specification;

public interface IDescribedMissionSpecification
{
    IModsetSetMissionSpecification WithModset(IBuildingModsetSpecification modsetSpecification);
}