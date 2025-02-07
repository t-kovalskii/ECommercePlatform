using ECommercePlatform.Services.User.Infrastructure.Entities;
using ECommercePlatform.Shared.Utils.DataAccess;
using ECommercePlatform.Shared.Utils.Entity;
using ECommercePlatform.Shared.Utils.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ECommercePlatform.Services.User.Infrastructure.Context;

public partial class ECommerceUsersContext : DbContext
{
    private const string DefaultConnectionStringName = "DefaultConnection";

    private readonly IConfiguration _configuration;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly ILogger _logger;
    
    public ECommerceUsersContext()
    {
    }

    public ECommerceUsersContext(DbContextOptions<ECommerceUsersContext> options,
        IConfiguration configuration,
        IDomainEventDispatcher domainEventDispatcher,
        ILogger logger)
        : base(options)
    {
        _configuration = configuration;
        _domainEventDispatcher = domainEventDispatcher;
        _logger = logger;
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Entities.User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(_configuration.GetConnectionString(DefaultConnectionStringName));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("user_status", new[] { "Active", "Deleted" });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("addresses_pkey");

            entity.ToTable("addresses");

            entity.HasIndex(e => e.UserId, "idx_addresses_user_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .HasColumnName("state");
            entity.Property(e => e.Street)
                .HasMaxLength(255)
                .HasColumnName("street");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(20)
                .HasColumnName("zip_code");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("addresses_user_id_fkey");
        });

        modelBuilder.Entity<Entities.User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "idx_users_email");

            entity.HasIndex(e => e.PhoneNumber, "idx_users_phone_number");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MerchantId).HasColumnName("merchant_id");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phone_number");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
