using System;
using ArmaForces.Boderator.Core.Infrastructure.Specifications;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;
using ArmaForces.Boderator.Core.Modsets.Specification;
using ArmaForces.Boderator.Core.Users;

namespace ArmaForces.Boderator.Core.Missions.Specifications;

public record MissionSpecification :
    IExpectMissionTitleSpecification,
    IExpectMissionDescriptionSpecification,
    IExpectModsetSpecification,
    IExpectMissionDateSpecification,
    IExpectMissionSignupsSpecification,
    IBuildingSpecification<Mission>
{
    private MissionSpecification() { }

    private User User { get; init; } = new();

    private string Title { get; init; } = string.Empty;

    private string Description { get; init; } = string.Empty;

    private IBuildingSpecification<Modset>? ModsetSpecification { get; init; }

    private DateTimeOffset Time { get; init; }

    private IBuildingSpecification<Signups>? SignupsSpecification { get; init; }

    public static IExpectMissionTitleSpecification OwnedBy(User user)
    {
        return new MissionSpecification
        {
            User = user
        };
    }

    public IExpectMissionDescriptionSpecification Titled(string title)
    {
        if (string.IsNullOrEmpty(title))
            throw new ArgumentException("Mission title cannot be null or empty", nameof(title));

        return this with
        {
            Title = title
        };
    }

    public IExpectModsetSpecification WithDescription(string description)
    {
        if (string.IsNullOrEmpty(description))
            throw new ArgumentException("Mission title cannot be null or empty", nameof(description));

        return this with
        {
            Description = description
        };
    }

    public IExpectMissionDateSpecification WithModset(IBuildingSpecification<Modset> modsetSpecification)
    {
        if (modsetSpecification is null)
            throw new ArgumentNullException(nameof(modsetSpecification));
        
        return this with
        {
            ModsetSpecification = modsetSpecification
        };
    }

    public IExpectMissionSignupsSpecification ScheduledAt(DateTimeOffset dateTime)
    {
        return this with
        {
            Time = dateTime
        };
    }

    public IBuildingSpecification<Mission> WithSignups(IBuildingSpecification<Signups> signupsSpecification)
    {
        if (signupsSpecification is null)
            throw new ArgumentNullException(nameof(signupsSpecification));
        
        return this with
        {
            SignupsSpecification = signupsSpecification
        };
    }

    public Mission Build()
        => new()
        {
            Owner = User.ToString(),
            Title = Title,
            Description = Description,
            MissionDate = Time.DateTime,
            ModsetName = ModsetSpecification!.Build().Name,
            Signups = SignupsSpecification!.Build()
        };
}