using Microsoft.EntityFrameworkCore;

using TbspRpgLib.Entities;

namespace TbspRpgLib.Repositories {
    public class ServiceContext : DbContext {
        public ServiceContext(DbContextOptions<ServiceContext> options) : base(options){}


        protected override void OnModelCreating(ModelBuilder modelBuilder){
        }
    }
}