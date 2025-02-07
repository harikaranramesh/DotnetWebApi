using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using IDMApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace IDMApi.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserServices> _logger;
        private readonly IConfiguration _configuration;

        public UserServices(ApplicationDbContext context, ILogger<UserServices> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<(string Token, string Role, string Username, int Id)> AuthenticateUser(User user)
        {
            var manager = await _context.Managers
                .FirstOrDefaultAsync(a => a.ManagerUsername == user.Username && a.ManagerPassword == user.Password);

            if (manager != null)
            {
                _logger.LogInformation("{ManagerName} logged in successfully.", manager.ManagerName);
                var token = GenerateJwtToken(manager.ManagerName, "Manager");
                return (token, "Manager", manager.ManagerName, manager.Id);
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Username == user.Username && e.Password == user.Password);

            if (employee != null)
            {
                _logger.LogInformation("{Username} logged in successfully.", employee.Username);
                var token = GenerateJwtToken(employee.Username, "Employee");
                return (token, "Employee", employee.Username, employee.EmployeeId);
            }

            _logger.LogWarning("Invalid login attempt for username: {Username}", user.Username);
            throw new UnauthorizedAccessException("Invalid login attempt for username: {Username}");
        }

        private string GenerateJwtToken(string username, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
