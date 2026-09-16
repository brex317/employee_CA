using Domain.Entities;
using Application.Interfaces;

namespace Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Employee>> GetAllAsync() => _repository.GetAllAsync();

    public Task<Employee?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

    public async Task<Employee> CreateAsync(Employee employee)
    {
        await _repository.AddAsync(employee);
        await _repository.SaveChangesAsync();
        return employee;
    }

    public async Task<bool> UpdateAsync(int id, Employee updatedEmployee)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee is null) return false;

        employee.FullName = updatedEmployee.FullName;
        employee.Email = updatedEmployee.Email;
        employee.Salary = updatedEmployee.Salary;
        employee.DepartmentId = updatedEmployee.DepartmentId;

        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee is null) return false;

        await _repository.DeleteAsync(employee);
        await _repository.SaveChangesAsync();
        return true;
    }
}