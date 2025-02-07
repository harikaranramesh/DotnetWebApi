using IDMApi.Application.Interfaces;
using IDMApi.Core.Interfaces;
using IDMApi.Models;

namespace IDMApi.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync() => await _employeeRepository.GetAllAsync();

        public async Task<Employee?> GetEmployeeByIdAsync(int id) => await _employeeRepository.GetByIdAsync(id);

        public async Task AddEmployeeAsync(Employee employee) => await _employeeRepository.AddAsync(employee);

        public async Task UpdateEmployeeAsync(Employee employee) => await _employeeRepository.UpdateAsync(employee);

        public async Task DeleteEmployeeAsync(int id) => await _employeeRepository.DeleteAsync(id);
    }
}
