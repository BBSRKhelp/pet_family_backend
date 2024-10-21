using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpeciesAggregate;
using PetFamily.Domain.SpeciesAggregate.ValueObjects.Ids;

namespace PetFamily.Infrastructure.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value));

        builder.ComplexProperty(s => s.Name, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });

        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey("species_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}