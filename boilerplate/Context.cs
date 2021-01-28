using Microsoft.EntityFrameworkCore;
using TbspRpgLib.Repositories;

namespace GameSystemApi.Repositories {
    public class GameSystemContext : ServiceTrackingContext {
        public GameSystemContext(DbContextOptions<GameSystemContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");
        }
    }
}