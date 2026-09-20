using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync() =>
        await _context.Employees.ToListAsync();

    public async Task<Employee?> GetByIdAsync(int id) =>
        await _context.Employees.FindAsync(id);

    public async Task<IEnumerable<Employee>> SearchAsync(string? name, decimal? minSalary)
{
    var query = _context.Employees.AsQueryable();

    if (!string.IsNullOrWhiteSpace(name))
        query = query.Where(e => e.FullName.Contains(name));

    if (minSalary.HasValue)
        query = query.Where(e => e.Salary >= minSalary.Value);

    return await query.ToListAsync();
}

    public async Task AddAsync(Employee employee) =>
        await _context.Employees.AddAsync(employee);

    public Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Employee employee)
    {
        _context.Employees.Remove(employee);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}