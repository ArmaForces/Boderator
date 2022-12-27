using System.Threading.Tasks;
using ArmaForces.Boderator.Core.Missions.Models;
using CSharpFunctionalExtensions;

namespace ArmaForces.Boderator.Core.Missions;

public interface ISignupsCommandService
{
    Task<Result<Signups>> CreateSignups(SignupsCreateRequest signupsCreateRequest);
}
