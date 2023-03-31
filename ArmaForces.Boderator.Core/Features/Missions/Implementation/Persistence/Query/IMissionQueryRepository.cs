using System.Collections.Generic;
using System.Threading.Tasks;
using ArmaForces.Boderator.Core.Common.Specifications;
using ArmaForces.Boderator.Core.Missions.Models;

namespace ArmaForces.Boderator.Core.Missions.Implementation.Persistence.Query;

internal interface IMissionQueryRepository
{
    Task<Mission?> GetMission(long missionId);
        
    Task<List<Mission>> GetMissions(IQuerySpecification<Mission>? query = null);
}
