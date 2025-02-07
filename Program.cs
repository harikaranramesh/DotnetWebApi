// Title : IDM
// Author : Harikaran
// Last Updated: 29/01/2025
// Reviewed by: Karthik

using IDMApi.Services;
using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IDMApi.Application.Interfaces;
using IDMApi.Application.Services;
using IDMApi.Core.Interfaces;
using IDMApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Register services in the DI container
builder.Services.AddHttpClient(); // Enables making HTTP requests to external services

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<ITaskServices, TaskServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();

// Enable Entity Framework Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register controllers
builder.Services.AddControllers();

// Set up Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

// Add distributed memory cache for session management
builder.Services.AddDistributedMemoryCache();

// Configure session settings (session timeout, cookie settings)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 30-minute idle timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Essential for maintaining session state
});

var app = builder.Build();

// Enable Swagger UI only in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔹 Correct order matters!
app.UseSession(); // Enable session middleware
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");

// Map API controllers
app.MapControllers();

// Run the application
app.Run();
