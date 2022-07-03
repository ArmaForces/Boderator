using System;
using ArmaForces.Boderator.Core.Infrastructure.Specifications;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Missions.Specifications.Interfaces;
using ArmaForces.Boderator.Core.Modsets.Specification;
using ArmaForces.Boderator.Core.Users;

namespace ArmaForces.Boderator.Core.Missions.Specifications;

public record MissionSpecification :
    IOwnedMissionSpecification,
    ITitledMissionSpecification,
    IDescribedMissionSpecification,
    IModsetSetMissionSpecification,
    IScheduledMissionSpecification,
    IBuildingSpecification<Models.Mission>
{
    private MissionSpecification() { }

    public User User { get; private init; } = new();
    
    public string Title { get; private init; } = string.Empty;

    public string Description { get; private init; } = string.Empty;

    public IBuildingSpecification<Modset>? ModsetSpecification { get; private init; }
    
    public DateTimeOffset Time { get; private init; }
    
    public IBuildingSpecification<Signups>? SignupsSpecification { get; private init; }

    public static IOwnedMissionSpecification OwnedBy(User user)
    {
        return new MissionSpecification
        {
            User = user
        };
    }

    public ITitledMissionSpecification WithTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
            throw new ArgumentException("Mission title cannot be null or empty", nameof(title));

        return this with
        {
            Title = title
        };
    }

    public IDescribedMissionSpecification WithDescription(string description)
    {
        if (string.IsNullOrEmpty(description))
            throw new ArgumentException("Mission title cannot be null or empty", nameof(description));

        return this with
        {
            Description = description
        };
    }

    public IModsetSetMissionSpecification WithModset(IBuildingSpecification<Modset> modsetSpecification)
    {
        if (modsetSpecification is null)
            throw new ArgumentNullException(nameof(modsetSpecification));
        
        return this with
        {
            ModsetSpecification = modsetSpecification
        };
    }

    public IScheduledMissionSpecification ScheduledAt(DateTimeOffset dateTime)
    {
        return this with
        {
            Time = dateTime
        };
    }

    public IBuildingSpecification<Models.Mission> WithSignups(IBuildingSpecification<Signups> signupsSpecification)
    {
        if (signupsSpecification is null)
            throw new ArgumentNullException(nameof(signupsSpecification));
        
        return this with
        {
            SignupsSpecification = signupsSpecification
        };
    }

    public Models.Mission Build()
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