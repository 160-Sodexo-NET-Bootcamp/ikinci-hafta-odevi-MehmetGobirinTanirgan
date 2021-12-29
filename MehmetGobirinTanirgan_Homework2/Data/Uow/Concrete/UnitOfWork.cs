using Data.Context;
using Data.Repositories.Abstract;
using Data.Repositories.Concrete;
using Data.Uow.Abstract;
using System.Threading.Tasks;

namespace Data.Uow.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SwcsDbContext context;

        public UnitOfWork(SwcsDbContext context)
        {
            this.context = context;
            Vehicles = new VehicleRepository(context);
            Containers = new ContainerRepository(context);
        }

        public IVehicleRepository Vehicles { get; private set; }
        public IContainerRepository Containers { get; private set; }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
