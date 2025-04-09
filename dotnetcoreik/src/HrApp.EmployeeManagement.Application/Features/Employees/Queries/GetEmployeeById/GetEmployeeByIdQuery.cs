// HrApp.EmployeeManagement.Application/Features/Employees/Queries/GetEmployeeById/GetEmployeeByIdQuery.cs

using HrApp.EmployeeManagement.Application.Features.Employees.Dtos; // EmployeeDetailVm için EKLENDİ
// using HrApp.EmployeeManagement.Domain.Entities; // Artık doğrudan gerekli değil
using MediatR;

namespace HrApp.EmployeeManagement.Application.Features.Employees.Queries.GetEmployeeById;

// --- DEĞİŞİKLİK: IRequest<TResponse> kısmındaki TResponse güncellendi ---
public class GetEmployeeByIdQuery : IRequest<EmployeeDetailVm?> // Artık nullable DTO dönüyor
{
    public Guid Id { get; private set; } // set'i private yapmak daha iyi olabilir

    public GetEmployeeByIdQuery(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Employee ID cannot be empty.", nameof(id));
        }
        Id = id;
    }
}