// src/HrApp.EmployeeManagement.Application/Features/Employees/Commands/DeleteEmployee/DeleteEmployeeCommand.cs
using MediatR;

namespace HrApp.EmployeeManagement.Application.Features.Employees.Commands.DeleteEmployee;

// --- DEĞİŞİKLİK: : IRequest eklendi ---
public class DeleteEmployeeCommand : IRequest // veya IRequest<Unit>
{
    public Guid Id { get; } // Constructor ile set ediliyorsa readonly olabilir

    public DeleteEmployeeCommand(Guid id)
    {
        Id = id;
    }
}