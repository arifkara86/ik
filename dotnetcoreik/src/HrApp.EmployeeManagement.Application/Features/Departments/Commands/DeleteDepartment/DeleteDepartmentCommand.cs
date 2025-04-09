// src/HrApp.EmployeeManagement.Application/Features/Departments/Commands/DeleteDepartment/DeleteDepartmentCommand.cs
using MediatR;

namespace HrApp.EmployeeManagement.Application.Features.Departments.Commands.DeleteDepartment;

// Silme işlemi de genellikle bir sonuç dönmez.
public class DeleteDepartmentCommand : IRequest // veya IRequest<Unit>
{
    public Guid Id { get; set; } // Hangi departmanın silineceği

    public DeleteDepartmentCommand(Guid id)
    {
        Id = id;
    }
}