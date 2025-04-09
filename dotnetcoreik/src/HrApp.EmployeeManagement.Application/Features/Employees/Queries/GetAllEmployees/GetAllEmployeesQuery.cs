// HrApp.EmployeeManagement.Application/Features/Employees/Queries/GetAllEmployees/GetAllEmployeesQuery.cs

using HrApp.EmployeeManagement.Application.Features.Employees.Dtos; // EmployeeListVm için EKLENDİ
using MediatR;
using System.Collections.Generic; // IEnumerable için EKLENDİ

namespace HrApp.EmployeeManagement.Application.Features.Employees.Queries.GetAllEmployees;

public class GetAllEmployeesQuery : IRequest<IReadOnlyList<EmployeeListVm>>
{
    // Bu sorgunun parametresi yok (şimdilik)
}