using ArmaForces.Boderator.Core.Infrastructure.Specifications;
using ArmaForces.Boderator.Core.Missions.Models;

namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface IExpectAnotherTeamSpecification
{
    bool CanAdd(IBuildingSpecification<Team>? team);
    
    IExpectAnotherTeamSpecification AndTeam(IBuildingSpecification<Team> team);
    
    IBuildingSpecification<Signups> AnoNoMoreTeams();
}
