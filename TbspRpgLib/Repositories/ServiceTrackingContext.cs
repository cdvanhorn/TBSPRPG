using Microsoft.EntityFrameworkCore;

using TbspRpgLib.Entities;

namespace TbspRpgLib.Repositories {
    public class ServiceTrackingContext : DbContext {
        public ServiceTrackingContext(DbContextOptions<ServiceContext> options) : base(options) {}

        protected ServiceTrackingContext(DbContextOptions options) : base(options) {}

        public DbSet<EventTypePosition> EventTypePostions { get; set; }

        public DbSet<ProcessedEvent> ProcessedEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<EventTypePosition>().ToTable("event_type_positions");
            modelBuilder.Entity<ProcessedEvent>().ToTable("processed_events");

            modelBuilder.Entity<EventTypePosition>().HasKey(a => a.Id);
            modelBuilder.Entity<EventTypePosition>().Property(a => a.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            modelBuilder.Entity<ProcessedEvent>().HasKey(a => a.Id);
            modelBuilder.Entity<ProcessedEvent>().Property(a => a.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();
        }
    }
}