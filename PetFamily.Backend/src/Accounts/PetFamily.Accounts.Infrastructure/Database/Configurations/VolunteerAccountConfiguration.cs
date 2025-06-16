using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Contracts.DTOs;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.Core.DTOs;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Infrastructure.Database.Configurations;

public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
{
    public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
    {
        builder.ToTable("volunteer_accounts");
        
        builder.HasKey(va => va.Id);
        
        builder.ComplexProperty(v => v.WorkExperience, web =>
        {
            web.Property(we => we.Value)
                .IsRequired()
                .HasColumnName("work_experience")
                .HasMaxLength(WorkExperience.MAX_NUMBER);
        });

        builder.Property(v => v.Requisites)
            .IsRequired()
            .ValueObjectsCollectionJsonConversion(
                r => new RequisiteDto(r.Title, r.Description),
                dto => Requisite.Create(dto.Title, dto.Description).Value);
        
        builder.Property(v => v.Certificates)
            .IsRequired()
            .ValueObjectsCollectionJsonConversion(
                c => new CertificateDto(c.Name, c.Url),
                dto => Certificate.Create(dto.Name, dto.Url).Value);
        
        builder.HasOne(va => va.User)
            .WithOne(u => u.VolunteerAccount)
            .HasForeignKey<VolunteerAccount>(va => va.UserId);
    }
}