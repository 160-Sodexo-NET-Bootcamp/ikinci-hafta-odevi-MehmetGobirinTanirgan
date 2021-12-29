using Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class SwcsDbContext : DbContext, ISwcsDbContext
    {
        public SwcsDbContext(DbContextOptions<SwcsDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Container> Containers { get; set; }
    }
}
