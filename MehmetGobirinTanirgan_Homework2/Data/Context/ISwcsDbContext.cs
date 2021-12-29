using Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public interface ISwcsDbContext
    {
        DbSet<Vehicle> Vehicles { get; set; }
        DbSet<Container> Containers { get; set; }
    }
}
