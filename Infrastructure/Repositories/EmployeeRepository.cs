
using IDMApi.Core.Interfaces;
using IDMApi.Models;
using MyApi.Data;

namespace IDMApi.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
