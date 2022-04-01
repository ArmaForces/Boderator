using System;
using Newtonsoft.Json;

namespace ArmaForces.Boderator.BotService.Features.Missions.DTOs;

public class MissionCreateRequestDto
{
    /// <summary>
    /// Mission title.
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Mission description.
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Mission start time.
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
    public DateTime? MissionTime { get; set; }

    /// <summary>
    /// Name of the modset.
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? ModsetName { get; set; }

    /// <summary>
    /// Owner of the mission.
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public string Owner { get; set; } = string.Empty;
}