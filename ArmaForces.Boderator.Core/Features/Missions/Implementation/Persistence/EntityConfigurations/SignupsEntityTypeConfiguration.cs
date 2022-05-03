using ArmaForces.Boderator.Core.Extensions;
using ArmaForces.Boderator.Core.Missions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArmaForces.Boderator.Core.Missions.Implementation.Persistence.EntityConfigurations;

internal class SignupsEntityTypeConfiguration : IEntityTypeConfiguration<Signups>
{
    public void Configure(EntityTypeBuilder<Signups> signupsConfiguration)
    {
        signupsConfiguration.HasKey(x => x.SignupsId);

        signupsConfiguration.Property(x => x.Status)
            .HasDefaultValue(SignupsStatus.Created)
            .HasConversion<ushort>()
            .IsRequired();

        signupsConfiguration.Property<long>("MissionId")
            .IsRequired();

        signupsConfiguration.HasOne<Mission>()
            .WithOne(x => x.Signups)
            .IsRequired();

        signupsConfiguration.HasMany<Team>()
            .WithOne()
            .IsNotRequired();
    }
}
