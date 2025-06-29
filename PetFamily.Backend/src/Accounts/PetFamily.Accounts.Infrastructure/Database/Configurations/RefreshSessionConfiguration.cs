using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain.DataModels;

namespace PetFamily.Accounts.Infrastructure.Database.Configurations;

public class RefreshSessionConfiguration : IEntityTypeConfiguration<RefreshSession>
{
    public void Configure(EntityTypeBuilder<RefreshSession> builder)
    {
        builder.HasKey(rs => rs.Id);
        
        builder.Property(rs => rs.Token)
            .IsRequired()
            .HasColumnOrder(2);
        
        builder.Property(rs => rs.Jti)
            .IsRequired()
            .HasColumnOrder(3);
        
        builder.Property(rs => rs.CreatedAt)
            .IsRequired()
            .HasColumnOrder(4);
        
        builder.Property(rs => rs.ExpiresAt)
            .IsRequired()
            .HasColumnOrder(5);
        
        builder.HasOne(rs => rs.User)
            .WithOne()
            .HasForeignKey<RefreshSession>(rs => rs.UserId);
    }
}