using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpeciesAggregate.Entities;
using PetFamily.Domain.SpeciesAggregate.ValueObjects.Ids;

namespace PetFamily.Infrastructure.Configurations;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breed");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .IsRequired()
            .HasConversion(id => id.Value,
                value => BreedId.Create(value));

        builder.ComplexProperty(b => b.Name, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });
    }
}