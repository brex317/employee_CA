using Api.Middleware;
using Application.Interfaces;
using Application.Services;
using Application.Settings;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Infrastructure
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddSingleton<IEmployeeCodeGenerator, EmployeeCodeGenerator>(); 

// Application
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<CompanySettings>(
    builder.Configuration.GetSection("CompanySettings")); 

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();