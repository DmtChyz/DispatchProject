using DispatchProject.Enumerable.ActionLog;
using LogMicroservice.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMicroservice.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ServiceLog> ServiceLogs { get; set; }
        public DbSet<DeadLetterLog> DeadLetterLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ServiceLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.OwnerId);
                entity.Property(e => e.ActionType)
                      .HasConversion<string>()
                      .HasMaxLength(50)
                      .IsRequired();
                entity.Property(e => e.Recipient).HasMaxLength(255);
                entity.Property(e => e.Details).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Status)
                      .HasConversion<string>()
                      .IsRequired();
                entity.Property(e => e.CorrelationId).HasMaxLength(100);
                entity.Property(e => e.CreatedAt).IsRequired();
            });
            modelBuilder.Entity<DeadLetterLog>(entity =>
            {
                entity.HasKey(e => e.Id);   
                entity.HasIndex(e => e.CorrelationId);
                entity.Property(e => e.QueueName).HasMaxLength(100);
                entity.Property(e => e.MessageBody).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Reason).HasColumnType("nvarchar(256)");
                entity.Property(e => e.ErrorDescription).HasColumnType("nvarchar(256)");
                entity.Property(e => e.EnqueuedAt).IsRequired();
                entity.Property(e => e.DeadLetteredAt).IsRequired();
            });
        }
    }
}
