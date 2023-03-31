using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmaForces.Boderator.Core.Common.Specifications;
using ArmaForces.Boderator.Core.Missions.Models;
using Microsoft.EntityFrameworkCore;

namespace ArmaForces.Boderator.Core.Missions.Implementation.Persistence.Query;

internal class MissionQueryRepository : IMissionQueryRepository
{
    private readonly MissionContext _context;
        
    public MissionQueryRepository(MissionContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Mission?> GetMission(long missionId)
        => await _context.Missions.FindAsync(missionId);

    public async Task<List<Mission>> GetMissions(IQuerySpecification<Mission>? query = null)
    {
        return await SpecificationEvaluator<Mission>.GetQuery(_context.Set<Mission>().AsQueryable(), query ?? new MissionQuerySpecification())
            .ToListAsync();
    }
}
