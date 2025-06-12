using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure.Database;

public class  AccountsDbContext(string connectionString) : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<AdminAccount> AdminAccounts => Set<AdminAccount>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("users");
            // .Property(u => u.SocialNetworks)
            // .HasConversion(
            //     sn => JsonSerializer.Serialize(sn, JsonSerializerOptions.Default),
            //     json => JsonSerializer.Deserialize<List<SocialNetwork>>(json, JsonSerializerOptions.Default)!);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();

        modelBuilder.Entity<AdminAccount>().ToTable("admin_accounts")
            .ComplexProperty(a => a.FullName, fb =>
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

        modelBuilder.Entity<AdminAccount>()
            .HasOne(a => a.User)
            .WithOne()
            .HasForeignKey<AdminAccount>(a => a.UserId);
        
        modelBuilder.Entity<Role>().ToTable("roles");

        modelBuilder.Entity<Permission>().ToTable("permissions")
            .HasIndex(p => p.Code)
            .IsUnique();

        modelBuilder.Entity<RolePermission>().ToTable("role_permissions")
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);

        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");

        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");

        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");

        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");

        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");

        // modelBuilder.ApplyConfigurationsFromAssembly(
        //     typeof(AccountsDbContext).Assembly);

        modelBuilder.HasDefaultSchema("accounts");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Information)
                .AddConsole();
        });
}