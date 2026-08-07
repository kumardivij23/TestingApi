using Microsoft.EntityFrameworkCore;
using TestingApi.Models;

namespace TestingApi.Data
{
    /// <summary>
    /// Application database context using EF Core InMemory provider for skill management.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<SkillAuditLog> SkillAuditLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.CertifyingAuthority).HasMaxLength(300);
                entity.HasIndex(e => e.PersonId);
                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<SkillAuditLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Details).HasMaxLength(2000);
                entity.HasIndex(e => e.SkillId);
                entity.HasIndex(e => e.PersonId);
            });
        }
    }
}
