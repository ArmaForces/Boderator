namespace ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;

public interface IExpectMissionTitleSpecification
{
    IExpectMissionDescriptionSpecification Titled(string title);
}