using ArmaForces.Boderator.Core.Extensions;
using ArmaForces.Boderator.Core.Missions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArmaForces.Boderator.Core.Missions.Implementation.Persistence.EntityConfigurations;

internal class MissionEntityTypeConfiguration : IEntityTypeConfiguration<Mission>
{
    public void Configure(EntityTypeBuilder<Mission> missionConfiguration)
    {
        missionConfiguration.HasKey(x => x.MissionId);

        missionConfiguration.Property(x => x.Title)
            .IsRequired();

        missionConfiguration.Property(x => x.Description)
            .IsNotRequired();

        missionConfiguration.Property(x => x.Owner)
            .IsRequired();
    }
}
