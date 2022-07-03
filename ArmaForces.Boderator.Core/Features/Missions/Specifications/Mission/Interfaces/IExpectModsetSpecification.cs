using ArmaForces.Boderator.Core.Infrastructure.Specifications;
using ArmaForces.Boderator.Core.Modsets.Specification;

namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface IExpectModsetSpecification
{
    IExpectMissionDateSpecification WithModset(IBuildingSpecification<Modset> modsetSpecification);
}