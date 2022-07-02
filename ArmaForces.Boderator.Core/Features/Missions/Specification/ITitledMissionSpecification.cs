namespace ArmaForces.Boderator.Core.Missions.Specification;

public interface ITitledMissionSpecification
{
    IDescribedMissionSpecification WithDescription(string description);
}