using Microsoft.EntityFrameworkCore;

using TbspRpgLib.Entities;

namespace TbspRpgLib.Repositories {
    public class ServiceTrackingContext : DbContext {
        public ServiceTrackingContext(DbContextOptions<ServiceContext> options) : base(options){}

        public DbSet<EventTypePosition> EventTypePostions { get; set; }

        public DbSet<ProcessedEvent> ProcessedEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
        }
    }
}