// src/HrApp.EmployeeManagement.Application/Features/Departments/Commands/CreateDepartment/CreateDepartmentCommandValidator.cs
using FluentValidation; // AbstractValidator, RuleFor için
using HrApp.EmployeeManagement.Application.Persistence; // IDepartmentRepository (benzersizlik kontrolü için)

namespace HrApp.EmployeeManagement.Application.Features.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    private readonly IDepartmentRepository _departmentRepository;

    public CreateDepartmentCommandValidator(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.") // Boş olamaz
            .NotNull()
            .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters."); // Maksimum uzunluk

        // Departman adının benzersiz (unique) olup olmadığını kontrol et
        RuleFor(p => p.Name)
            .MustAsync(BeUniqueName).WithMessage("A department with the same name already exists.");
    }

    // Veritabanına bakarak ismin benzersiz olup olmadığını kontrol eden asenkron metot
    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        // Repository kullanarak bu isimde başka departman var mı diye bak
        var existingDepartment = await _departmentRepository.GetFirstOrDefaultAsync(d => d.Name.ToLower() == name.ToLower(), cancellationToken);
        return existingDepartment == null; // Eğer yoksa (null ise) true (benzersizdir) döner.
    }
}