// src/HrApp.EmployeeManagement.Application/Features/Employees/Dtos/CreateEmployeeDto.cs
using System.ComponentModel.DataAnnotations; // DataAnnotations ekleyebiliriz (FluentValidation'a ek olarak)

namespace HrApp.EmployeeManagement.Application.Features.Employees.Dtos;

public class CreateEmployeeDto
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public DateTime HireDate { get; set; }

    [StringLength(100)]
    public string? Position { get; set; }

    public Guid? DepartmentId { get; set; } // Nullable Guid

    [Range(0, double.MaxValue)]
    public decimal Salary { get; set; } // Non-nullable decimal
}