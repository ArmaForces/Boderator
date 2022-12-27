using System.Threading.Tasks;
using ArmaForces.Boderator.Core.Missions.Models;
using CSharpFunctionalExtensions;

namespace ArmaForces.Boderator.Core.Missions.Implementation.Persistence.Command;

internal interface ISignupsCommandRepository
{
    Task<Result<Signups>> CreateSignups(long missionId, Signups signups);
}