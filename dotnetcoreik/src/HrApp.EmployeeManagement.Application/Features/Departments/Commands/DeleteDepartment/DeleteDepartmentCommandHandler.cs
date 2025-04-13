// src/HrApp.EmployeeManagement.Application/Features/Departments/Commands/DeleteDepartment/DeleteDepartmentCommandHandler.cs
using HrApp.EmployeeManagement.Application.Exceptions;
using HrApp.EmployeeManagement.Application.Persistence; // IUnitOfWork için
using HrApp.EmployeeManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.EmployeeManagement.Application.Features.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand>
{
    private readonly IUnitOfWork _unitOfWork; // EKLENDİ
    private readonly ILogger<DeleteDepartmentCommandHandler> _logger;

    public DeleteDepartmentCommandHandler(
        IUnitOfWork unitOfWork, // EKLENDİ
        ILogger<DeleteDepartmentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork; // EKLENDİ
        _logger = logger;
    }

    public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
         _logger.LogInformation("Attempting to delete department with ID: {DepartmentId}", request.Id);
        var departmentToDelete = await _unitOfWork.DepartmentRepository.GetByIdAsync(request.Id, cancellationToken);

        if (departmentToDelete == null)
        {
             _logger.LogWarning("Department with ID: {DepartmentId} not found for deletion.", request.Id);
            throw new NotFoundException(nameof(Department), request.Id);
        }

        // Repository'deki kontrol (çalışan var mı vs.) hala geçerli olabilir.
        _unitOfWork.DepartmentRepository.Delete(departmentToDelete);

        // --- DEĞİŞİKLİK: SaveChangesAsync eklendi ---
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Database changes saved for deleted department {DepartmentId}.", request.Id);
        // --- Bitiş: DEĞİŞİKLİK ---

        _logger.LogInformation("Department with ID: {DepartmentId} successfully deleted.", request.Id);
        // return Unit.Value;
    }
}