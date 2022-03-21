using System.Collections.Generic;
using System.Linq;
using ArmaForces.Boderator.BotService.Features.Missions.DTOs;
using ArmaForces.Boderator.Core.Missions.Models;

namespace ArmaForces.Boderator.BotService.Features.Missions.Mappers
{
    public static class MissionMapper
    {
        public static MissionDto Map(Mission mission)
            => new()
            {
                Title = mission.Title,
                MissionDate = mission.MissionTime,
                MissionId = mission.MissionId
            };

        public static List<MissionDto> Map(List<Mission> missions)
            => missions.Select(Map).ToList();

        public static MissionCreateRequest Map(MissionCreateRequestDto request)
            => new MissionCreateRequest
            {
                Title = request.Title,
                Description = request.Description,
                Owner = request.Owner,
                ModsetName = request.ModsetName,
                MissionTime = request.MissionTime
            };
    }
}