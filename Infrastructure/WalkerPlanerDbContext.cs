using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class WalkerPlanerDbContext : DbContext
    {
        public WalkerPlanerDbContext(DbContextOptions<WalkerPlanerDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Dog>()
                .HasIndex(a => a.Name)
                .IsUnique();

            // Many-to-many: Session <-> Dogs
            modelBuilder
                .Entity<SessionDog>()
                .HasKey(ca => new { ca.SessionId, ca.DogId });

            // Many-to-many: Walker <-> Session
            modelBuilder
                .Entity<SessionWalker>()
                .HasKey(ss => new { ss.SessionId, ss.WalkerId });
        }

        public DbSet<Dog> Dogs { get; set; } = default!;

        public DbSet<Walker> Walkers { get; set; } = default!;

        public DbSet<Track> Tracks { get; set; } = default!;

        public DbSet<Owner> Owners { get; set; } = default!;

        public DbSet<Session> Session { get; set; } = default!;
    }
}
