using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
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
                .HasKey(ca => new { ca.SessionId, ca.AttendeeId });

            // Many-to-many: Walker <-> Session
            modelBuilder
                .Entity<SessionWalker>()
                .HasKey(ss => new { ss.SessionId, ss.SpeakerId });
        }

        public DbSet<Dog> Dogs { get; set; } = default!;

        public DbSet<Walker> Walker { get; set; } = default!;

        public DbSet<Track> Tracks { get; set; } = default!;

        public DbSet<Owner> Owners { get; set; } = default!;

        public DbSet<Session> Session { get; set; } = default!;
    }
}
