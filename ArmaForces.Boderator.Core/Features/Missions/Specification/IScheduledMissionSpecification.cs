namespace ArmaForces.Boderator.Core.Missions.Specification;

public interface IScheduledMissionSpecification
{
    IBuildingMissionSpecification WithSignups(IBuildingSignupsSpecification signupsSpecification);
}