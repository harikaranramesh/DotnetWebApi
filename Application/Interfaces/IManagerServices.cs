using IDMApi.Models;

namespace IDMApi.Services
{
    public interface IManagerService
    {
        Task<IEnumerable<Manager>> GetAllManagerAsync();

        Task<Manager?> GetManagerByIdAsync(int id);
        Task AddManager(Manager manager);
        Task DeleteManager(int id);
    }
}           