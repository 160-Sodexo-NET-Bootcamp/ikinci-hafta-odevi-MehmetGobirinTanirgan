using Data.Context;
using Data.DataModels;
using Data.Repositories.Abstract;

namespace Data.Repositories.Concrete
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(SwcsDbContext context) : base(context)
        {
        }

    }
}
