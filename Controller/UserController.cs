using Microsoft.AspNetCore.Mvc;
using System.Text;
using MyApi.Data;
using IDMApi.Models;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(ILogger<UserController> _logger, IUserServices _userServices) : Controller
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var (token, role, username, id) = await _userServices.AuthenticateUser(user);

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Invalid username or password.");
                }

                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetInt32("Id", id);

                HttpContext.Response.Cookies.Append("TOKEN", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                });

                _logger.LogInformation($"{username} logged in successfully.");
                return Ok(new { Token = token, Username = username, Role = role, Id = id });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "An error occurred while logging in.");
                return StatusCode(401, "Invalid UserName or Password");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in.");
                return StatusCode(500, "Internal Server Error");
            }
            
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            string? username = HttpContext.Session.GetString("Username");

            _logger.LogInformation($"{username ?? "User"} logged out successfully.");
            Response.Cookies.Delete("TOKEN");
            HttpContext.Session.Clear();

            return Ok("Logged out successfully.");
        }
    }
}
