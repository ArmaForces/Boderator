using System;
using ArmaForces.Boderator.Core.Missions.Models;
using ArmaForces.Boderator.Core.Modsets.Specification;
using ArmaForces.Boderator.Core.Users;

namespace ArmaForces.Boderator.Core.Missions.Specification;

public record MissionSpecification :
    IOwnedMissionSpecification,
    ITitledMissionSpecification,
    IDescribedMissionSpecification,
    IModsetSetMissionSpecification,
    IScheduledMissionSpecification,
    IBuildingMissionSpecification
{
    private MissionSpecification() { }

    private User User { get; init; } = new();
    
    private string Title { get; init; } = string.Empty;

    private string Description { get; init; } = string.Empty;

    private IBuildingModsetSpecification? ModsetSpecification { get; init; }
    
    private DateTimeOffset Time { get; init; }
    
    private IBuildingSignupsSpecification? SignupsSpecification { get; init; }

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

    public IModsetSetMissionSpecification WithModset(IBuildingModsetSpecification modsetSpecification)
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

    public IBuildingMissionSpecification WithSignups(IBuildingSignupsSpecification signupsSpecification)
    {
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