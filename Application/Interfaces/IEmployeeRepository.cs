using Domain.Entities;

namespace Application.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<IEnumerable<Employee>> SearchAsync(string? name, decimal? minSalary);
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Employee employee);
    Task SaveChangesAsync();
}