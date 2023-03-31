﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ArmaForces.Boderator.Core.Common.Specifications;
using ArmaForces.Boderator.Core.Missions.Implementation.Persistence;
using ArmaForces.Boderator.Core.Missions.Implementation.Persistence.Query;
using ArmaForces.Boderator.Core.Missions.Models;
using CSharpFunctionalExtensions;

namespace ArmaForces.Boderator.Core.Missions.Implementation;

internal class MissionQueryService : IMissionQueryService
{
    private readonly IMissionQueryRepository _missionQueryRepository;
    
    public MissionQueryService(IMissionQueryRepository missionQueryRepository)
    {
        _missionQueryRepository = missionQueryRepository;
    }
    
    public async Task<Result<Mission>> GetMission(long missionId)
        => await _missionQueryRepository.GetMission(missionId)
           ?? Result.Failure<Mission>($"Mission with ID {missionId} does not exist.");

    public async Task<Result<List<Mission>>> GetMissions(IQuerySpecification<Mission>? query = null)
        => await _missionQueryRepository.GetMissions(query);
}
