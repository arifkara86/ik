// src/HrApp.EmployeeManagement.Application/Features/Departments/Queries/GetAllDepartments/GetAllDepartmentsQuery.cs
using HrApp.EmployeeManagement.Application.Features.Departments.Dtos; // DepartmentDto için
using MediatR; // IRequest için

namespace HrApp.EmployeeManagement.Application.Features.Departments.Queries.GetAllDepartments;

// Bu sorgu işlendiğinde DepartmentDto listesi dönecek
public class GetAllDepartmentsQuery : IRequest<IReadOnlyList<DepartmentDto>>
{
    // Bu sorgunun parametresi yok (şimdilik)
    // İleride filtreleme, sayfalama parametreleri eklenebilir (örn: string SearchTerm, int PageNumber, int PageSize)
}