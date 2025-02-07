using IDMApi.Core.Interfaces;
using IDMApi.Models;
using IDMApi.Services;

namespace IDMApi.Application.Services
{
    public class ManagerService(IManagerRepository _managerRepository) : IManagerService
    {

        public async Task AddManager(Manager manager)
        {
            await _managerRepository.AddAsync(manager);
        }

        public async Task DeleteManager(int id)
        {
            await _managerRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Manager>> GetAllManagerAsync()
        {
            return await _managerRepository.GetAllAsync();
        }

        public async Task<Manager?> GetManagerByIdAsync(int id)
        {
            return await _managerRepository.GetByIdAsync(id);
        }
    }
}