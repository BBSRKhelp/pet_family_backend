using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain.DataModels;

namespace PetFamily.Accounts.Infrastructure.Database.Configurations;

public class ParticipantAccountConfiguration : IEntityTypeConfiguration<ParticipantAccount>
{
    public void Configure(EntityTypeBuilder<ParticipantAccount> builder)
    {
        builder.ToTable("participant_accounts");

        builder.HasKey(pa => pa.Id);

        builder.Property(pa => pa.FavoritePetIds)
            .IsRequired()
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<IReadOnlyList<Guid>>(v, JsonSerializerOptions.Default)!,
                new ValueComparer<IReadOnlyList<Guid>>(
                    (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                )
            )
            .HasColumnName("favorite_pets")
            .HasColumnType("jsonb");

        builder.HasOne(pa => pa.User)
            .WithOne(u => u.ParticipantAccount)
            .HasForeignKey<ParticipantAccount>(pa => pa.UserId);
    }
}