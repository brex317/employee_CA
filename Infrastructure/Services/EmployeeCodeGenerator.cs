using Application.Interfaces;

namespace Infrastructure.Services;

public class EmployeeCodeGenerator : IEmployeeCodeGenerator
{
    private int _counter = 1000;

    public string GenerateCode()
    {
        _counter++;
        return $"EMP-{_counter}";
    }
}