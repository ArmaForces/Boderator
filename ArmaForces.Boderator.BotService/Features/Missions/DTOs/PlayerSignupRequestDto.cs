﻿namespace ArmaForces.Boderator.BotService.Features.Missions.DTOs;

public record PlayerSignupRequestDto
{
    public long SlotId { get; set; }

    public string Player { get; set; } = string.Empty;
}