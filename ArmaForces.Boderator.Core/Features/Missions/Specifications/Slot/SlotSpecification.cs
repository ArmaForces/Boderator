using System;
using ArmaForces.Boderator.Core.Infrastructure.Specifications;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Users;

namespace ArmaForces.Boderator.Core.Missions.Specifications;

public record SlotSpecification :
    IExpectOptionalSlotOccupantSpecification,
    IExpectSlotOptionalVehicleSpecification,
    IBuildingSpecification<Slot>
{
    private SlotSpecification() { }

    private string Name { get; init; } = string.Empty;
    
    private string? Occupant { get; init; }
    
    private string? Vehicle { get; init; }

    public static IExpectOptionalSlotOccupantSpecification Named(string slotName)
    {
        if (string.IsNullOrEmpty(slotName))
            throw new ArgumentException("Slot name cannot be null or empty.");

        return new SlotSpecification
        {
            Name = slotName
        };
    }

    public static IBuildingSpecification<Slot> UnoccupiedWithoutVehicleNamed(string slotName)
    {
        if (string.IsNullOrEmpty(slotName))
            throw new ArgumentException("Slot name cannot be null or empty.");

        return new SlotSpecification
        {
            Name = slotName
        };
    }

    public IExpectSlotOptionalVehicleSpecification OccupiedBy(User user) =>
        this with
        {
            Occupant = user.Name
        };

    public IExpectSlotOptionalVehicleSpecification Unoccupied() => this;

    public IBuildingSpecification<Slot> WithVehicle(string vehicleName)
    {
        if (string.IsNullOrEmpty(vehicleName))
            throw new ArgumentException("Specified vehicle name is null or empty.");
        
        return this with
        {
            Vehicle = vehicleName
        };
    }

    public IBuildingSpecification<Slot> WithoutVehicle() => this;

    public Slot Build()
    {
        return new Slot
        {
            Name = Name,
            Occupant = Occupant,
            Vehicle = Vehicle
        };
    }
}

public interface IExpectSlotOptionalVehicleSpecification
{
    IBuildingSpecification<Slot> WithVehicle(string vehicleName);

    IBuildingSpecification<Slot> WithoutVehicle();
}

public interface IExpectOptionalSlotOccupantSpecification
{
    IExpectSlotOptionalVehicleSpecification OccupiedBy(User user);
    
    IExpectSlotOptionalVehicleSpecification Unoccupied();
}
