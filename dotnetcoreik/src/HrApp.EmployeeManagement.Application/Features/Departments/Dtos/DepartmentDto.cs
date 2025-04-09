// src/HrApp.EmployeeManagement.Application/Features/Departments/Dtos/DepartmentDto.cs
namespace HrApp.EmployeeManagement.Application.Features.Departments.Dtos;

public class DepartmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    // İleride bu departmandaki çalışan sayısını veya başka özet bilgileri ekleyebiliriz.
    // public int EmployeeCount { get; set; }
}