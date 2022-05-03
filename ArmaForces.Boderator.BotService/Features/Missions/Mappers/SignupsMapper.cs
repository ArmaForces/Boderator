﻿using System.Collections.Generic;
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
}
