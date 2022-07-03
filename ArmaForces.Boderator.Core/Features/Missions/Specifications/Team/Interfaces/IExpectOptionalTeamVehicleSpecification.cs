namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface IExpectOptionalTeamVehicleSpecification
{
    IExpectSlotSpecification WithVehicle(string vehicleName);

    IExpectSlotSpecification WithoutVehicle();
}
