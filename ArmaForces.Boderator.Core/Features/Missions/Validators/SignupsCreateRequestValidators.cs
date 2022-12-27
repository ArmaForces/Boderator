using ArmaForces.Boderator.Core.Missions.Implementation;
using ArmaForces.Boderator.Core.Missions.Models;
using CSharpFunctionalExtensions;

namespace ArmaForces.Boderator.Core.Missions.Validators;

internal static class SignupsCreateRequestValidators
{
    public static Result ValidateRequest(this SignupsCreateRequest signupsCreateRequest)
    {
        if (signupsCreateRequest.MissionId.HasValue && signupsCreateRequest.MissionCreateRequest is not null)
        {
            return Result.Failure($"Only one of {nameof(SignupsCreateRequest.MissionId)} and {nameof(SignupsCreateRequest.MissionCreateRequest)} can be specified.");
        }

        if (signupsCreateRequest.MissionCreateRequest is not null)
            return signupsCreateRequest.MissionCreateRequest.ValidateRequest();

        if (signupsCreateRequest.MissionId > 0)
            return Result.Success();

        return Result.Failure("Signups creation request validation failure");
    }
}
