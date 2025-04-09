// src/HrApp.EmployeeManagement.Application/Features/Employees/Commands/UpdateEmployee/UpdateEmployeeCommandValidator.cs

using FluentValidation;
using HrApp.EmployeeManagement.Application.Persistence;

namespace HrApp.EmployeeManagement.Application.Features.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public UpdateEmployeeCommandValidator(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;

        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} must be a valid GUID.");

        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .EmailAddress().WithMessage("A valid {PropertyName} is required.")
            .MaximumLength(150).WithMessage("{PropertyName} must not exceed 150 characters.");

         RuleFor(p => p)
             .MustAsync(BeUniqueEmailWhenUpdating)
             .WithMessage("An employee with the same email already exists.");

        RuleFor(p => p.DateOfBirth)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .LessThan(DateTime.Today).WithMessage("{PropertyName} must be in the past.");

        RuleFor(p => p.Position)
            .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

        // --- DÜZELTİLMİŞ ALANLAR ---
        RuleFor(p => p.Salary)
             // UpdateEmployeeCommand.Salary nullable (decimal?) olduğu için .HasValue KULLANILIR.
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be zero or positive.")
            .When(p => p.Salary.HasValue); // Kural sadece Salary null değilse çalışır.

        RuleFor(p => p.DepartmentId) // 'Department' DEĞİL, 'DepartmentId'
            .MustAsync(BeValidDepartment).WithMessage("Invalid Department ID specified.")
            .When(p => p.DepartmentId.HasValue && p.DepartmentId != Guid.Empty);
        // --- BİTİŞ ---
    }

    private async Task<bool> BeUniqueEmailWhenUpdating(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
         var existingEmployee = await _employeeRepository.GetFirstOrDefaultAsync(e => e.Email.ToLower() == command.Email.ToLower() && e.Id != command.Id, cancellationToken);
         return existingEmployee == null;
    }

    private async Task<bool> BeValidDepartment(Guid? departmentId, CancellationToken cancellationToken)
    {
        if (!departmentId.HasValue || departmentId == Guid.Empty)
        {
            return true;
        }
        var department = await _departmentRepository.GetByIdAsync(departmentId.Value, cancellationToken);
        return department != null;
    }
}