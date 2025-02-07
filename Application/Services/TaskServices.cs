using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using IDMApi.Models;

namespace IDMApi.Services
{
    public class TaskServices : ITaskServices
    {
        private readonly ApplicationDbContext _context;

        public TaskServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetAssignedTask(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);

                if (employee == null)
                {
                    throw new KeyNotFoundException($"Employee with ID {id} not found.");
                }

                return employee;
            }
            catch (Exception ex)
            {
                // Log exception (optional, add logging dependency if needed)
                // _logger.LogError(ex, "Error fetching task for employee with ID {id}", id);
                throw new Exception($"An error occurred while fetching the task for Employee ID {id}: {ex.Message}");
            }
        }

        public async Task AssignTask(int id, string task)
        {
            try
            {
                var existingEmployee = await _context.Employees.FindAsync(id);

                if (existingEmployee == null)
                {
                    throw new KeyNotFoundException($"Employee with ID {id} not found.");
                }

                if (string.IsNullOrEmpty(task))
                {
                    throw new ArgumentException("Task cannot be empty.");
                }

                existingEmployee.Tasks = task;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database-specific errors
                throw new Exception("A database error occurred while assigning the task.", dbEx);
            }
            catch (ArgumentException argEx)
            {
                // Handle invalid task argument
                throw new ArgumentException(argEx.Message, argEx);
            }
            catch (Exception ex)
            {
                // Handle general errors
                throw new Exception($"An error occurred while assigning the task for Employee ID {id}: {ex.Message}", ex);
            }
        }
    }
}