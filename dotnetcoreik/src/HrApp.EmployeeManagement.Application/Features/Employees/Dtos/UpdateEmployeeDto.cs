// src/HrApp.EmployeeManagement.Application/Features/Employees/Dtos/UpdateEmployeeDto.cs
using System.ComponentModel.DataAnnotations;

namespace HrApp.EmployeeManagement.Application.Features.Employees.Dtos;

public class UpdateEmployeeDto
{
    // Güncelleme DTO'sunda genellikle ID bulunmaz (URL'den alınır),
    // ama bazen validasyon veya tutarlılık için eklenebilir. Şimdilik eklemeyelim.
    // public Guid Id { get; set; }

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

    [StringLength(100)]
    public string? Position { get; set; }

    public Guid? DepartmentId { get; set; } // Nullable Guid

    [Range(0, double.MaxValue)]
    public decimal? Salary { get; set; } // Nullable decimal
}