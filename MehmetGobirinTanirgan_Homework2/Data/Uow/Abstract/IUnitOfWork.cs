using Data.Repositories.Abstract;
using System.Threading.Tasks;

namespace Data.Uow.Abstract
{
    public interface IUnitOfWork
    {
        IVehicleRepository Vehicles { get; }
        IContainerRepository Containers { get; }
        Task SaveAsync();
        void Dispose();
    }
}
