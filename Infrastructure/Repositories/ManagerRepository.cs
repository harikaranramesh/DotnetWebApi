using IDMApi.Core.Interfaces;
using IDMApi.Models;
using MyApi.Data;


namespace IDMApi.Infrastructure.Repositories
{
    public class ManagerRepository : Repository<Manager>, IManagerRepository
    {
        public ManagerRepository(ApplicationDbContext context) : base(context) { }
    }
}
