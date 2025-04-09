// src/HrApp.EmployeeManagement.Application/Features/Employees/Commands/UpdateEmployee/UpdateEmployeeCommand.cs
using MediatR;

namespace HrApp.EmployeeManagement.Application.Features.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommand : IRequest
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? Position { get; set; }
    public Guid? DepartmentId { get; set; } // Nullable Guid
    public decimal? Salary { get; set; } // Nullable decimal
}