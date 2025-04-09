// src/HrApp.EmployeeManagement.Application/Features/Departments/Queries/GetDepartmentById/GetDepartmentByIdQuery.cs
using HrApp.EmployeeManagement.Application.Features.Departments.Dtos; // DepartmentDto için
using MediatR; // IRequest için

namespace HrApp.EmployeeManagement.Application.Features.Departments.Queries.GetDepartmentById;

// Bu sorgu işlendiğinde tek bir DepartmentDto dönecek (veya bulunamazsa Handler null/exception dönebilir)
public class GetDepartmentByIdQuery : IRequest<DepartmentDto?> // Nullable Dto?
{
    public Guid Id { get; set; }

    // Constructor ile ID'yi almak iyi bir pratik olabilir
    public GetDepartmentByIdQuery(Guid id)
    {
        Id = id;
    }
}