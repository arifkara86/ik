// HrApp.EmployeeManagement.Application/Features/Employees/Commands/DeleteEmployee/DeleteEmployeeCommandValidator.cs

using FluentValidation;

namespace HrApp.EmployeeManagement.Application.Features.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommandValidator : AbstractValidator<DeleteEmployeeCommand>
{
    public DeleteEmployeeCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("{PropertyName} is required.");
    }
}