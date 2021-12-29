using Data.Context;
using Data.DataModels;
using Data.Repositories.Abstract;

namespace Data.Repositories.Concrete
{
    public class ContainerRepository : GenericRepository<Container>, IContainerRepository
    {
        public ContainerRepository(SwcsDbContext context) : base(context)
        {
        }
    }
}
