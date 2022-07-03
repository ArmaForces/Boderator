using ArmaForces.Boderator.Core.Infrastructure.Specifications;
using ArmaForces.Boderator.Core.Missions.Models;

namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface IExpectSlotSpecification
{
    bool CanAdd(IBuildingSpecification<Slot>? slot);
    
    IExpectAnotherSlotSpecification WithSlot(IBuildingSpecification<Slot> slot);
}