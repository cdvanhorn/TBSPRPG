using Microsoft.EntityFrameworkCore;

using TbspRpgLib.Entities;

namespace TbspRpgLib.Repositories {
    public class ServiceContext : DbContext {
        public ServiceContext(DbContextOptions<ServiceContext> options) : base(options){}

        // public DbSet<Service> Services { get; set; }

        // public DbSet<EventType> EventTypes { get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder){
        // }
    }
}