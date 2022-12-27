using System.Linq;
using ArmaForces.Boderator.Core.Missions.Models;
using CSharpFunctionalExtensions;

namespace ArmaForces.Boderator.Core.Missions.Validators;

internal static class MissionCreateRequestValidators
{
    public static Result ValidateRequest(this MissionCreateRequest missionCreateRequest)
    {
        if (missionCreateRequest.ModsetName?.Any(char.IsWhiteSpace) ?? false)
            return Result.Failure($"{nameof(MissionCreateRequest.ModsetName)} cannot contain whitespace characters.");
        
        return Result.Success();
    }
}
