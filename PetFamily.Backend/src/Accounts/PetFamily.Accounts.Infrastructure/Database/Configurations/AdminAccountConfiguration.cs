using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain.DataModels;

namespace PetFamily.Accounts.Infrastructure.Database.Configurations;

public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable("admin_accounts");
        
        builder.HasKey(aa => aa.Id);
        
        builder.HasOne(aa => aa.User)
            .WithOne(u => u.AdminAccount)
            .HasForeignKey<AdminAccount>(aa => aa.UserId);
    }
}