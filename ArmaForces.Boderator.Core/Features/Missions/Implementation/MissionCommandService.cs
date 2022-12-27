using System.Linq;
using System.Threading.Tasks;
using ArmaForces.Boderator.Core.Missions.Implementation.Persistence.Command;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Missions.Validators;
using CSharpFunctionalExtensions;

namespace ArmaForces.Boderator.Core.Missions.Implementation;

internal class MissionCommandService : IMissionCommandService
{
    private readonly IMissionCommandRepository _missionCommandRepository;

    public MissionCommandService(IMissionCommandRepository missionCommandRepository)
    {
        _missionCommandRepository = missionCommandRepository;
    }

    public async Task<Result<Mission>> CreateMission(MissionCreateRequest missionCreateRequest)
    {
        return await missionCreateRequest.ValidateRequest()
            .Bind(() => _missionCommandRepository.CreateMission(
            new Mission
            {
                Title = missionCreateRequest.Title,
                Description = missionCreateRequest.Description,
                Owner = missionCreateRequest.Owner,
                MissionDate = missionCreateRequest.MissionDate,
                ModsetName = missionCreateRequest.ModsetName
            }));
    }
}