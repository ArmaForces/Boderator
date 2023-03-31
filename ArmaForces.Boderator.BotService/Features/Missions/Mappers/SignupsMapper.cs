using System.Collections.Generic;
using System.Linq;
using ArmaForces.Boderator.BotService.Features.Missions.DTOs;
using ArmaForces.Boderator.Core.Missions.Models;

namespace ArmaForces.Boderator.BotService.Features.Missions.Mappers;

public static class SignupsMapper
{
    public static SignupsDto Map(Signups signups)
        => new()
        {
            SignupsId = signups.SignupsId,
            Status = signups.Status,
            StartDate = signups.StartDate,
            CloseDate = signups.CloseDate,
            Teams = Map(signups.Teams)
        };
    
    public static TeamDto Map(Team team)
        => new()
        {
            Name = team.Name,
            Slots = Map(team.Slots),
            Vehicle = team.Vehicle
        };

    public static List<TeamDto> Map(IEnumerable<Team> teams)
        => teams.Select(Map).ToList();

    public static List<Team> Map(IEnumerable<TeamDto> teams)
        => teams.Select(Map).ToList();

    public static Team Map(TeamDto team)
        => new()
        {
            Name = team.Name,
            Vehicle = team.Vehicle,
            Slots = Map(team.Slots)
        };

    public static SlotDto Map(Slot slot)
        => new()
        {
            SlotId = slot.SlotId,
            Name = slot.Name,
            Occupant = slot.Occupant,
            Vehicle = slot.Vehicle
        };

    public static List<SlotDto> Map(IEnumerable<Slot> slots)
        => slots.Select(Map).ToList();

    public static List<Slot> Map(IEnumerable<SlotDto> slots)
        => slots.Select(Map).ToList();

    public static Slot Map(SlotDto slot) => new()
    {
        SlotId = slot.SlotId,
        Name = slot.Name,
        Occupant = slot.Occupant,
        Vehicle = slot.Vehicle
    };

    public static SignupsCreateRequest Map(SignupsCreateRequestDto request)
    {
        return new SignupsCreateRequest
        {
            MissionId = request.MissionId,
            MissionCreateRequest = request.Mission is not null ? new MissionCreateRequest
            {
                Title = request.Mission.Title,
                Description = request.Mission.Description,
                Owner = request.Mission.Owner,
                MissionDate = request.Mission.MissionDate,
                ModsetName = request.Mission.ModsetName
            } : null,
            StartDate = request.StartDate,
            CloseDate = request.CloseDate,
            SignupsStatus = request.SignupsStatus,
            Teams = Map(request.Teams)
        };
    }
}
