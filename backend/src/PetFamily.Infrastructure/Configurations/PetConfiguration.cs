using PetFamily.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.SpeciesAggregate.ValueObjects.Ids;
using PetFamily.Domain.VolunteerAggregate.Entities;
using PetFamily.Domain.VolunteerAggregate.Enums;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Ids;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value));
        
        builder.Property(p => p.Nickname)
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
            .IsRequired(false);
        
        builder.Property(p => p.Description)
            .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
            .IsRequired(false);

        builder.ComplexProperty(p => p.AppearanceDetails, ab =>
        {
            ab.Property(ad => ad.SpeciesId)
                .IsRequired()
                .HasColumnName("species_id")
                .HasConversion(
                    id => id.Value,
                    value => SpeciesId.Create(value));
            
            ab.Property(ad => ad.BreedId)
                .IsRequired()
                .HasColumnName("breed_id")
                .HasConversion(
                    id => id.Value,
                    value => BreedId.Create(value));
            
            ab.Property(ad => ad.Coloration)
                .IsRequired()
                .HasColumnName("coloration")
                .HasDefaultValue(Colour.Unknown);
            
            ab.Property(ad => ad.Weight)
                .IsRequired()
                .HasColumnName("weight")
                .HasDefaultValue(0);
            
            ab.Property(ad => ad.Height)
                .IsRequired()
                .HasColumnName("height")
                .HasDefaultValue(0);
        });

        builder.ComplexProperty(p => p.HealthDetails, hdb =>
        {
            hdb.Property(hd => hd.HealthInformation)
                .IsRequired()
                .HasColumnName("health_information")
                .HasMaxLength(Constants.MAX_MEDIUM_LOW_TEXT_LENGTH);
            
            hdb.Property(hd => hd.IsCastrated)
                .IsRequired()
                .HasColumnName("is_castrated")
                .HasDefaultValue(false);

            hdb.Property(hd => hd.IsVaccinated)
                .IsRequired()
                .HasColumnName("is_vaccinated")
                .HasDefaultValue(false);
        });
        
        builder.OwnsOne(p => p.Address, ab =>
        {
            ab.Property(p => p.Country)
                .IsRequired()
                .HasColumnName("country")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            
            ab.Property(p => p.City)
                .IsRequired()
                .HasColumnName("city")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            
            ab.Property(p => p.Street)
                .IsRequired()
                .HasColumnName("street")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            
            ab.Property(p => p.Postalcode)
                .IsRequired(false)
                .HasColumnName("postalcode")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });
        
        builder.Property(p => p.PhoneNumber)
            .IsRequired()
            .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
        
        builder.Property(p => p.Status)
            .IsRequired()
            .HasDefaultValue(StatusForHelp.Unknown);

        builder.OwnsOne(p => p.Details, db =>
        {
            db.ToJson("details");

            db.OwnsMany(d => d.Requisites, rb =>
            {
                rb.Property(r => r.Title)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
                
                rb.Property(r => r.Description)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_MEDIUM_LOW_TEXT_LENGTH);
            });

            db.OwnsMany(d => d.PetPhotos, pb =>
            {
               pb.Property(p => p.Path)
                   .IsRequired()
                   .HasMaxLength(Constants.MAX_MEDIUM_HIGH_TEXT_LENGTH);

               pb.Property(p => p.IsMainPhoto)
                   .IsRequired();
            });
        });
    }
} 