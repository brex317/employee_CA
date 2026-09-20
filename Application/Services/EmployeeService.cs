using Microsoft.Extensions.Options;
using Domain.Entities;
using Application.Interfaces;
using Application.DTOs;
using Application.Exceptions;
using Application.Settings; 



namespace Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly IEmployeeCodeGenerator _codeGenerator;
    private readonly CompanySettings _companySettings; 

    public EmployeeService(IEmployeeRepository repository ,
     IEmployeeCodeGenerator codeGenerator,
     IOptions<CompanySettings> companySettings
     )
    {
        _repository = repository;
        _codeGenerator = codeGenerator; 
         _companySettings = companySettings.Value;
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetAllAsync()
    {
        var employees = await _repository.GetAllAsync();
        return employees.Select(MapToResponseDto);
    }

    public async Task<EmployeeResponseDto?> GetByIdAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);
        return employee is null ? null : MapToResponseDto(employee);
    }

    public async Task<IEnumerable<EmployeeResponseDto>> SearchAsync(string? name, decimal? minSalary)
{
    var employees = await _repository.SearchAsync(name, minSalary);
    return employees.Select(MapToResponseDto);
}

    public async Task<EmployeeResponseDto> CreateAsync(CreateEmployeeDto dto)
    {
        var employeesInDept = await _repository.SearchAsync(null, null);
        var countInDept = employeesInDept.Count(e => e.DepartmentId == dto.DepartmentId);

        if (countInDept >= _companySettings.MaxEmployeesPerDepartment)
            throw new ValidationException(
                $"Department {dto.DepartmentId} has reached the maximum of {_companySettings.MaxEmployeesPerDepartment} employees");
        
        var employee = new Employee
        {
            EmployeeCode = _codeGenerator.GenerateCode(),
            FullName = dto.FullName,
            Email = dto.Email,
            Salary = dto.Salary,
            DepartmentId = dto.DepartmentId
            
        };

        await _repository.AddAsync(employee);
        await _repository.SaveChangesAsync();

        return MapToResponseDto(employee);
    }

    public async Task<bool> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee is null)
            throw new NotFoundException($"Employee with id {id} was not found"); 


        employee.FullName = dto.FullName;
        employee.Email = dto.Email;
        employee.Salary = dto.Salary;
        employee.DepartmentId = dto.DepartmentId;

        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee is null)
            throw new NotFoundException($"Employee with id {id} was not found");

        await _repository.DeleteAsync(employee);
        await _repository.SaveChangesAsync();
        return true;
    }

    private static EmployeeResponseDto MapToResponseDto(Employee employee)
    {
        return new EmployeeResponseDto
        {
            Id = employee.Id,
              EmployeeCode = employee.EmployeeCode,
            FullName = employee.FullName,
            Email = employee.Email,
            Salary = employee.Salary,
            DepartmentId = employee.DepartmentId
        };
    }
}