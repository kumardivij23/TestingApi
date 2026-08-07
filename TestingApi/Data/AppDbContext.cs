using Microsoft.EntityFrameworkCore;
using TestingApi.Models;

namespace TestingApi.Data
{
    /// <summary>
    /// Application database context for Entity Framework Core.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// CCOG unified skill records.
        /// </summary>
        public DbSet<CCOG_Skill> CCOG_Skills { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CCOG_Skill>(entity =>
            {
                entity.ToTable("CCOG_Skills");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.SkillName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CertificationName)
                    .HasMaxLength(300);

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Index on Owner for fast lookups
                entity.HasIndex(e => e.Owner);

                // Index on Category for filtered queries
                entity.HasIndex(e => e.Category);

                // Index on IsActive for soft-delete filtering
                entity.HasIndex(e => e.IsActive);
            });
        }
    }
}
