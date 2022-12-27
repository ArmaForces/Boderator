using System;
using System.Threading.Tasks;
using ArmaForces.Boderator.Core.Missions.Implementation.Persistence.Command;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Missions.Validators;
using CSharpFunctionalExtensions;

namespace ArmaForces.Boderator.Core.Missions.Implementation;

internal class SignupsCommandService : ISignupsCommandService
{
    private readonly IMissionCommandService _missionCommandService;
    private readonly ISignupsCommandRepository _signupsCommandRepository;
    
    public SignupsCommandService(
        IMissionCommandService missionCommandService,
        ISignupsCommandRepository signupsCommandRepository)
    {
        _missionCommandService = missionCommandService;
        _signupsCommandRepository = signupsCommandRepository;
    }

    public async Task<Result<Signups>> CreateSignups(SignupsCreateRequest signupsCreateRequest)
    {
        return await signupsCreateRequest.ValidateRequest()
            .Bind(() => CreateOrGetMissionId(signupsCreateRequest))
            .Bind(missionId => _signupsCommandRepository.CreateSignups(missionId, new Signups
            {
                Status = signupsCreateRequest.SignupsStatus,
                StartDate = signupsCreateRequest.StartDate ??
                            (signupsCreateRequest.SignupsStatus == SignupsStatus.Open ? DateTime.Now : null),
                CloseDate = signupsCreateRequest.CloseDate,
                Teams = signupsCreateRequest.Teams
            }));
    }

    private async Task<Result<long>> CreateOrGetMissionId(SignupsCreateRequest signupsCreateRequest)
    {
        if (signupsCreateRequest.MissionCreateRequest is not null)
            return await _missionCommandService.CreateMission(signupsCreateRequest.MissionCreateRequest)
                .Map(x => x.MissionId);
        return Result.Success(signupsCreateRequest.MissionId!.Value);
    }
}