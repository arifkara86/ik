// src/HrApp.EmployeeManagement.Application/Features/Departments/Dtos/CreateDepartmentDto.cs
using System.ComponentModel.DataAnnotations; // Geçici olarak DataAnnotations ekleyebiliriz, FluentValidation asıl yer olacak.

namespace HrApp.EmployeeManagement.Application.Features.Departments.Dtos;

public class CreateDepartmentDto
{
    [Required(ErrorMessage = "Department name is required.")] // FluentValidation'a ek olarak API seviyesinde de kontrol sağlar.
    [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;
}