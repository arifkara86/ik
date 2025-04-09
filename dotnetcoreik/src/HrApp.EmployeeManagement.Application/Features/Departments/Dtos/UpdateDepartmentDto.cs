// src/HrApp.EmployeeManagement.Application/Features/Departments/Dtos/UpdateDepartmentDto.cs
using System.ComponentModel.DataAnnotations;

namespace HrApp.EmployeeManagement.Application.Features.Departments.Dtos;

public class UpdateDepartmentDto
{
    // Güncellenecek departmanın ID'si genellikle URL'den gelir,
    // ama bazen body içinde de gönderilmesi istenebilir veya validasyon için kullanılır.
    // Şimdilik sadece güncellenecek alanı ekleyelim.
    // public Guid Id { get; set; } // Controller'da URL'den gelenle karşılaştırmak için eklenebilir.

    [Required(ErrorMessage = "Department name is required.")]
    [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;
}