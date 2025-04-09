using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteer.Contracts.DTOs;
using PetFamily.Volunteer.Contracts.DTOs.Volunteer;
using PetFamily.Volunteer.Domain.ValueObjects;
using PetFamily.Volunteer.Domain.ValueObjects.Ids;

namespace PetFamily.Volunteer.Infrastructure.Database.Configurations.Write;

public class VolunteerConfiguration : IEntityTypeConfiguration<Domain.Volunteer>
{
    public void Configure(EntityTypeBuilder<Domain.Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value));

        builder.ComplexProperty(v => v.FullName, fb =>
        {
            fb.Property(f => f.FirstName)
                .IsRequired()
                .HasColumnName("first_name")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

            fb.Property(f => f.LastName)
                .IsRequired()
                .HasColumnName("last_name")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

            fb.Property(f => f.Patronymic)
                .IsRequired(false)
                .HasColumnName("patronymic")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });

        builder.ComplexProperty(v => v.Email, eb =>
        {
            eb.Property(e => e.Value)
                .IsRequired()
                .HasColumnName("email")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });

        builder.ComplexProperty(v => v.Description, db =>
        {
            db.Property(d => d.Value)
                .IsRequired(false)
                .HasColumnName("description")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });

        builder.ComplexProperty(v => v.WorkExperience, web =>
        {
            web.Property(we => we.Value)
                .IsRequired()
                .HasColumnName("work_experience")
                .HasMaxLength(WorkExperience.MAX_NUMBER);
        });

        builder.ComplexProperty(p => p.PhoneNumber, pb =>
        {
            pb.Property(pn => pn.Value)
                .IsRequired()
                .HasColumnName("phone_number")
                .HasMaxLength(PhoneNumber.MAX_LENGTH);
        });

        builder.Property(v => v.SocialNetworks)
            .IsRequired()
            .ValueObjectsCollectionJsonConversion(
                sn => new SocialNetworkDto(sn.Title, sn.Url),
                dto => SocialNetwork.Create(dto.Title, dto.Url).Value);

        builder.Property(v => v.Requisites)
            .IsRequired()
            .ValueObjectsCollectionJsonConversion(
                r => new RequisiteDto(r.Title, r.Description),
                dto => Requisite.Create(dto.Title, dto.Description).Value);

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(v => v.Pets).AutoInclude();
    }
}