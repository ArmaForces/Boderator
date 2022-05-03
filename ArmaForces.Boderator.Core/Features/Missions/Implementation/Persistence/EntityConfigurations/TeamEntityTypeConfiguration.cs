using ArmaForces.Boderator.Core.Extensions;
using ArmaForces.Boderator.Core.Missions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArmaForces.Boderator.Core.Missions.Implementation.Persistence.EntityConfigurations;

internal class TeamEntityTypeConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> teamConfiguration)
    {
        teamConfiguration.HasKey("SignupsId", "Name");
        
        teamConfiguration.Property<long>("SignupsId")
            .IsRequired();

        teamConfiguration.Property<string>("Vehicle")
            .IsNotRequired();

        teamConfiguration.HasOne<Signups>()
            .WithMany(x => x.Teams)
            .HasForeignKey("SignupsId")
            .IsRequired();
    }
}
