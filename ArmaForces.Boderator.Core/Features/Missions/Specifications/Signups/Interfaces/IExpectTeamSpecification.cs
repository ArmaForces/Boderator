using ArmaForces.Boderator.Core.Infrastructure.Specifications;
using ArmaForces.Boderator.Core.Missions.Models;

namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface IExpectTeamSpecification
{
    bool CanAdd(IBuildingSpecification<Team>? team);
    
    IExpectAnotherTeamSpecification WithTeam(IBuildingSpecification<Team> team);
}
