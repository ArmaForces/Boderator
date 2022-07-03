namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface IExpectMissionDescriptionSpecification
{
    IExpectModsetSpecification WithDescription(string description);
}