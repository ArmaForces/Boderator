using System.Collections.Generic;
using ArmaForces.Boderator.Core.Infrastructure.Specifications;
using ArmaForces.Boderator.Core.Missions.Models;

namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface IExpectAnotherSlotSpecification
{
    bool CanAdd(IBuildingSpecification<Slot>? slot);
    
    IExpectAnotherSlotSpecification AndSlot(IBuildingSpecification<Slot> slot);

    IExpectAnotherSlotSpecification WithSlots(List<IBuildingSpecification<Slot>> slots);
    
    IBuildingSpecification<Team> AndNoMoreSlots();
}
