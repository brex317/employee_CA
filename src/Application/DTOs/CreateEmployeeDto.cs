using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CreateEmployeeDto : IValidatableObject
{
    [Required(ErrorMessage = "Full name is required")]
    [MaxLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Range(1000, 1000000, ErrorMessage = "Salary must be between 1,000 and 1,000,000")]
    public decimal Salary { get; set; }

    public int DepartmentId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DepartmentId == 1 && Salary < 80000)
        {
            yield return new ValidationResult(
                "Employees in the Management department (DepartmentId 1) must have a salary of at least 80,000",
                new[] { nameof(Salary) }
            );
        }
    }
}