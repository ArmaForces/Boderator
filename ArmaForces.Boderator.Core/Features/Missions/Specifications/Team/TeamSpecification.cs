using System;
using System.Collections.Generic;
using System.Linq;
using ArmaForces.Boderator.Core.Infrastructure.Specifications;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

namespace ArmaForces.Boderator.Core.Missions.Specifications;

public record TeamSpecification :
    IExpectSlotSpecification,
    IExpectAnotherSlotSpecification,
    IExpectOptionalTeamVehicleSpecification,
    IBuildingSpecification<Team>
{
    private TeamSpecification() { }

    private string Name { get; init; } = string.Empty;

    private string? Vehicle { get; init; }

    private IReadOnlyList<IBuildingSpecification<Slot>> Slots { get; init; } = new List<IBuildingSpecification<Slot>>();

    public static IExpectOptionalTeamVehicleSpecification Named(string teamName)
    {
        if (string.IsNullOrEmpty(teamName))
            throw new ArgumentException("Team name cannot be empty", nameof(teamName));

        return new TeamSpecification
        {
            Name = teamName
        };
    }

    public IExpectSlotSpecification WithVehicle(string vehicleName)
    {
        if (string.IsNullOrEmpty(vehicleName))
            throw new ArgumentException("Specified vehicle name is null or empty");
        
        return this with
        {
            Vehicle = vehicleName
        };
    }

    public IExpectSlotSpecification WithoutVehicle() => this;

    public bool CanAdd(IBuildingSpecification<Slot>? slot)
    {
        var builtSlotToAdd = slot?.Build();
        
        return builtSlotToAdd is not null && Slots.All(x =>
        {
            var builtSlot = x.Build();
            return (builtSlotToAdd.SlotId is null ||
                    builtSlot.SlotId != builtSlotToAdd.SlotId) &&
                   (builtSlot.Occupant is null ||
                    builtSlot.Occupant != builtSlotToAdd.Occupant);
        });
    }


    public IExpectAnotherSlotSpecification WithSlot(IBuildingSpecification<Slot> slot) => AddSlot(slot);

    public IExpectAnotherSlotSpecification WithSlots(List<IBuildingSpecification<Slot>> slots)
        => slots.Aggregate(this, (currentTeam, slot) => currentTeam.AddSlot(slot));

    public IExpectAnotherSlotSpecification AndSlot(IBuildingSpecification<Slot> slot) => AddSlot(slot);

    public IBuildingSpecification<Team> AndNoMoreSlots() => this;

    public Team Build()
    {
        return new Team
        {
            Name = Name,
            Vehicle = Vehicle,
            Slots = Slots.Select(x => x.Build()).ToList()
        };
    }

    private TeamSpecification AddSlot(IBuildingSpecification<Slot> slot)
    {
        if (!CanAdd(slot))
            throw new ArgumentException("Slot cannot be added due to slot ID or occupant duplication.");
        
        return this with
        {
            Slots = new List<IBuildingSpecification<Slot>>(Slots) {slot}
        };
    }
}