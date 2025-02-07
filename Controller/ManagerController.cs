using IDMApi.Application.Interfaces;
using IDMApi.Models;
using IDMApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace IDMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController(IManagerService _managerService, ITaskServices _taskServices, ILogger<ManagerController> _logger) : ControllerBase
    {
        // Get All Managers
        [HttpGet("getallmanagers")]
        public async Task<IActionResult> GetAllManagers()
        {
            try
            {
                var allManagers = await _managerService.GetAllManagerAsync();
                return Ok(allManagers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching managers.");
                return StatusCode(500, new { message = "An error occurred while fetching managers.", error = ex.Message });
            }
        }

        // Add Manager
        [HttpPost("addmanager")]
        public async Task<IActionResult> AddManager([FromBody] Manager manager)
        {
            try
            {
                if (manager == null)
                {
                    return BadRequest(new { message = "Manager data is required." });
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _managerService.AddManager(manager);
                return Ok(new { message = "Manager added successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Invalid data provided.", error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding manager.");
                return StatusCode(500, new { message = "An error occurred while adding the manager.", error = ex.Message });
            }
        }

        // Delete Manager
        [HttpDelete("deletemanager/{id}")]
        public async Task<IActionResult> DeleteManager(int id)
        {
            try
            {
                var manager = await _managerService.GetManagerByIdAsync(id);
                if (manager == null)
                {
                    return NotFound(new { message = "Manager not found" });
                }

                await _managerService.DeleteManager(id);
                return Ok(new { message = "Manager deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting manager.");
                return StatusCode(500, new { message = "Error occurred while deleting manager.", error = ex.Message });
            }
        }

        //Get Manager by ID
        [HttpGet("getmanagerbyid/{id}")]
        public async Task<IActionResult> GetManagerProfile(int id)
        {
            try
            {
                var manager = await _managerService.GetManagerByIdAsync(id);
                if (manager == null)
                {
                    return NotFound(new { message = "Manager not found." });
                }
                return Ok(manager);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching manager profile.");
                return StatusCode(500, new { message = "Error occurred while fetching manager profile.", error = ex.Message });
            }
        }

        // Get Assigned Task for Manager
        [HttpGet("getassignedtask/{id}")]
        public async Task<IActionResult> GetAssignedTaskAsync(int id)
        {
            try
            {
                var task = await _taskServices.GetAssignedTask(id);
                if (task == null)
                {
                    return NotFound(new { message = "Task not found for this manager." });
                }
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching assigned task.");
                return StatusCode(500, new { message = "Error occurred while fetching assigned task.", error = ex.Message });
            }
        }

        // Assign Task to Manager
        [HttpPatch("assigntask/{id}")]
        public async Task<IActionResult> AssignTask(int id, [FromBody] string task)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(task))
                {
                    return BadRequest(new { message = "Task description is required." });
                }

                await _taskServices.AssignTask(id, task);
                return Ok(new { message = "Task assigned successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while assigning task.");
                return StatusCode(500, new { message = "Error occurred while assigning task.", error = ex.Message });
            }
        }
    }
}
