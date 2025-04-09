// src/HrApp.EmployeeManagement.Application/Features/Departments/Commands/UpdateDepartment/UpdateDepartmentCommandValidator.cs
using FluentValidation;
using HrApp.EmployeeManagement.Application.Persistence;

namespace HrApp.EmployeeManagement.Application.Features.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    private readonly IDepartmentRepository _departmentRepository;

    public UpdateDepartmentCommandValidator(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;

        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("{PropertyName} is required.") // ID boş olamaz
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} must be a valid GUID."); // ID geçersiz GUID olamaz

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

        // Güncelleme sırasında da isim benzersizliğini kontrol etmeliyiz,
        // AMA güncellenen kaydın *kendisi hariç* diğer kayıtlarla çakışmamalı.
        RuleFor(p => p) // Tüm komutu alıyoruz (hem Id hem Name lazım)
           .MustAsync(BeUniqueNameWhenUpdating).WithMessage("A department with the same name already exists.");
    }

     // Güncelleme için benzersizlik kontrolü
    private async Task<bool> BeUniqueNameWhenUpdating(UpdateDepartmentCommand command, CancellationToken cancellationToken)
    {
         // Bu isimde AMA FARKLI ID'de başka departman var mı?
        var existingDepartment = await _departmentRepository.GetFirstOrDefaultAsync(d => d.Name.ToLower() == command.Name.ToLower() && d.Id != command.Id, cancellationToken);
        return existingDepartment == null; // Eğer yoksa (null ise) true (benzersizdir veya kendi adıdır) döner.
    }
}