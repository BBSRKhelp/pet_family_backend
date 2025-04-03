using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Volunteer.Contracts.DTOs;
using PetFamily.Volunteer.Contracts.DTOs.Volunteer;

namespace PetFamily.Volunteer.Infrastructure.Database.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(x => x.Id);

        builder.Property(v => v.SocialNetworks)
            .HasConversion(
                values => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<SocialNetworkDto[]>(json, JsonSerializerOptions.Default)!);

        builder.Property(v => v.Requisites)
            .HasConversion(
                values => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<RequisiteDto[]>(json, JsonSerializerOptions.Default)!);
    }
}