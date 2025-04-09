// src/HrApp.EmployeeManagement.Application/Features/Employees/Commands/CreateEmployee/CreateEmployeeCommand.cs
using MediatR;

namespace HrApp.EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommand : IRequest<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime HireDate { get; set; }
    public string? Position { get; set; }
    public Guid? DepartmentId { get; set; } // Nullable Guid
    public decimal Salary { get; set; } // Non-nullable decimal
}