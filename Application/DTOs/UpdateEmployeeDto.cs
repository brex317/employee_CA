using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class UpdateEmployeeDto : IValidatableObject
{
    [Required(ErrorMessage = "Full name is required")]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Range(1000, 1000000)]
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