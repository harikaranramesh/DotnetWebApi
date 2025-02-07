using IDMApi.Application.Interfaces;
using IDMApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace IDMApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController(IEmployeeService employeeService) : ControllerBase
    {
        private readonly IEmployeeService _employeeService = employeeService;

        //Get All Employees
        [HttpGet("getallemployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new{message = "An error occurred while fetching employees.",error = ex.Message});
            }
        }

        // Get Employee by ID
        [HttpGet("getemployeebyid/{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    return NotFound(new { message = "Employee not found" });
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while fetching the employee.",
                    error = ex.Message
                });
            }
        }

        // Add Employee
        [HttpPost("addemployee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest(new { message = "Invalid employee data" });
                }

                await _employeeService.AddEmployeeAsync(employee);
                return Ok(new { message = "Employee added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while adding the employee.",
                    error = ex.Message
                });
            }
        }

        // Delete Employee
        [HttpDelete("deleteemployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    return NotFound(new { message = "Employee not found" });
                }

                await _employeeService.DeleteEmployeeAsync(id);
                return Ok(new { message = "Employee deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while deleting the employee.",
                    error = ex.Message
                });
            }
        }

        // Update Employee
        [HttpPut("updateemployee/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest(new { message = "Invalid employee data." });
                }

                if (id != employee.EmployeeId)
                {
                    return BadRequest(new { message = "Employee ID mismatch." });
                }

                var existingEmployee = await _employeeService.GetEmployeeByIdAsync(id);
                if (existingEmployee == null)
                {
                    return NotFound(new { message = "Employee not found" });
                }

                await _employeeService.UpdateEmployeeAsync(employee);
                return Ok(new { message = "Employee updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while updating the employee.",
                    error = ex.Message
                });
            }
        }
    }
}
