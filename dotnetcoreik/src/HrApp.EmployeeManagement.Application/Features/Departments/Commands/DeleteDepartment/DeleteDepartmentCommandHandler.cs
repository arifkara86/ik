// src/HrApp.EmployeeManagement.Application/Features/Departments/Commands/DeleteDepartment/DeleteDepartmentCommandHandler.cs
using HrApp.EmployeeManagement.Application.Exceptions;
using HrApp.EmployeeManagement.Application.Persistence; // IUnitOfWork için
using HrApp.EmployeeManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.EmployeeManagement.Application.Features.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand>
{
    private readonly IUnitOfWork _unitOfWork; // DEĞİŞTİ
    private readonly ILogger<DeleteDepartmentCommandHandler> _logger;

    public DeleteDepartmentCommandHandler(
        IUnitOfWork unitOfWork, // DEĞİŞTİ
        ILogger<DeleteDepartmentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork; // DEĞİŞTİ
        _logger = logger;
    }

    public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
         _logger.LogInformation("Attempting to delete department with ID: {DepartmentId}", request.Id);
        var departmentToDelete = await _unitOfWork.DepartmentRepository.GetByIdAsync(request.Id, cancellationToken); // UoW Kullanıldı

        if (departmentToDelete == null)
        {
             _logger.LogWarning("Department with ID: {DepartmentId} not found for deletion.", request.Id);
            throw new NotFoundException(nameof(Department), request.Id);
        }

        // Repository'deki silme öncesi kontrol hala geçerli olabilir (opsiyonel)
        _unitOfWork.DepartmentRepository.Delete(departmentToDelete); // UoW Kullanıldı

        await _unitOfWork.SaveChangesAsync(cancellationToken); // UoW Kullanıldı
        _logger.LogInformation("Database changes saved for deleted department {DepartmentId}.", request.Id);

        _logger.LogInformation("Department with ID: {DepartmentId} successfully deleted.", request.Id);
        // return Unit.Value;
    }
}