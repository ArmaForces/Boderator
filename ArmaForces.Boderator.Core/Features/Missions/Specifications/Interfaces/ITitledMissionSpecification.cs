namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface ITitledMissionSpecification
{
    IDescribedMissionSpecification WithDescription(string description);
}