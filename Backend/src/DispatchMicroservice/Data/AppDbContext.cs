using DispatchMicroservice.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Enumerable;

namespace DispatchProject.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<NotificationOperation> NotificationOperations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NotificationOperation>(entity =>
            {
                entity.ToTable("NotificationOperations");

                entity.HasKey(x => x.CorrelationId);

                entity.Property(x => x.CorrelationId)
                    .HasMaxLength(32)
                    .IsRequired();

                entity.Property(x => x.UserId)
                    .IsRequired();

                entity.Property(x => x.Status)
                    .HasConversion<string>()
                    .HasMaxLength(32)
                    .IsRequired();

                entity.Property(x => x.CreatedAtUtc)
                    .HasColumnType("datetime2")
                    .IsRequired();

                entity.Property(x => x.UpdatedAtUtc)
                    .HasColumnType("datetime2")
                    .IsRequired();

                entity.HasIndex(x => x.UserId);

                entity.HasIndex(x => new
                {
                    x.UserId,
                    x.CreatedAtUtc
                });

                entity.HasOne<IdentityUser>()
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}