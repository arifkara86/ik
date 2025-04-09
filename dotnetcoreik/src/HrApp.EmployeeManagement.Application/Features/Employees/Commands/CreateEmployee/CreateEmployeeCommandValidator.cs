// src/HrApp.EmployeeManagement.Application/Features/Employees/Commands/CreateEmployee/CreateEmployeeCommandValidator.cs

using FluentValidation;
using HrApp.EmployeeManagement.Application.Persistence;

namespace HrApp.EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public CreateEmployeeCommandValidator(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;

        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .EmailAddress().WithMessage("A valid {PropertyName} is required.")
            .MaximumLength(150).WithMessage("{PropertyName} must not exceed 150 characters.")
            .MustAsync(BeUniqueEmail).WithMessage("An employee with the same email already exists."); // Benzersizlik kontrolü

        RuleFor(p => p.DateOfBirth)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .LessThan(DateTime.Today).WithMessage("{PropertyName} must be in the past.");

         RuleFor(p => p.HireDate)
            .NotEmpty().WithMessage("{PropertyName} is required.");
            // .LessThanOrEqualTo(DateTime.Today).WithMessage("{PropertyName} cannot be in the future."); // İşe giriş gelecekte olamaz

        RuleFor(p => p.Position)
            .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

        // --- DÜZELTİLMİŞ ALANLAR ---
        RuleFor(p => p.Salary)
             // CreateEmployeeCommand.Salary non-nullable decimal olduğu için .HasValue KULLANILMAZ.
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be zero or positive.");

        RuleFor(p => p.DepartmentId) // 'Department' DEĞİL, 'DepartmentId'
            .MustAsync(BeValidDepartment).WithMessage("Invalid Department ID specified.")
            .When(p => p.DepartmentId.HasValue && p.DepartmentId != Guid.Empty);
        // --- BİTİŞ ---
    }

    // Email benzersizlik kontrolü (oluşturma)
    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
         var existingEmployee = await _employeeRepository.GetFirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower(), cancellationToken);
         return existingEmployee == null;
    }

    // Departman ID geçerlilik kontrolü
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