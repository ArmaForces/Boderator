using System;
using System.Threading.Tasks;
using ArmaForces.Boderator.Core.Missions.Models;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace ArmaForces.Boderator.Core.Missions.Implementation.Persistence.Command;

internal class SignupsCommandRepository : ISignupsCommandRepository
{
    private readonly MissionContext _context;

    public SignupsCommandRepository(MissionContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Result<Signups>> CreateSignups(long missionId, Signups signups)
    {
        var signupsEntityEntry = await _context.Signups.AddAsync(signups);

        if (signupsEntityEntry is null) return Result.Failure<Signups>("Failure creating signups.");

        var mission = await _context.Missions.AsNoTracking().SingleOrDefaultAsync(x => x.MissionId == missionId);
        if (mission is null) return Result.Failure<Signups>($"Mission with ID {missionId} doesn't exist.");
        
        var updatedMission = mission with
        {
            Signups = signupsEntityEntry.Entity
        };

        _context.Attach(updatedMission);
        _context.Entry(updatedMission).Reference(x => x.Signups).IsModified = true;

        await _context.SaveChangesAsync();
        return signupsEntityEntry.Entity;
    }
}
