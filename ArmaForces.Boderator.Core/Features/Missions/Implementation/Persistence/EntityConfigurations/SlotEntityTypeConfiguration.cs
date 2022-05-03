using ArmaForces.Boderator.Core.Extensions;
using ArmaForces.Boderator.Core.Missions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArmaForces.Boderator.Core.Missions.Implementation.Persistence.EntityConfigurations;

internal class SlotEntityTypeConfiguration : IEntityTypeConfiguration<Slot>
{
    public void Configure(EntityTypeBuilder<Slot> slotConfiguration)
    {
        slotConfiguration.HasKey(x => x.SlotId);

        slotConfiguration.Property<string>("Name")
            .IsRequired();

        slotConfiguration.Property<string>("Vehicle")
            .IsNotRequired();

        slotConfiguration.Property<string>("Occupant")
            .IsNotRequired();

        slotConfiguration.HasOne<Team>()
            .WithMany(x => x.Slots)
            .HasForeignKey("SignupsId", "TeamName");
    }
}
