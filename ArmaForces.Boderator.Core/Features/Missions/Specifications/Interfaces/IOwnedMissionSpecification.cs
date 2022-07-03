namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface IOwnedMissionSpecification
{
    ITitledMissionSpecification WithTitle(string title);
}