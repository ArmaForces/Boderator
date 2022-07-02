namespace ArmaForces.Boderator.Core.Missions.Specification;

public interface IOwnedMissionSpecification
{
    ITitledMissionSpecification WithTitle(string title);
}