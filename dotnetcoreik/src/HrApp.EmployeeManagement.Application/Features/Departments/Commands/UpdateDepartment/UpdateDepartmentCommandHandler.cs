// src/HrApp.EmployeeManagement.Application/Features/Departments/Commands/UpdateDepartment/UpdateDepartmentCommandHandler.cs
using AutoMapper;
using HrApp.EmployeeManagement.Application.Exceptions;
using HrApp.EmployeeManagement.Application.Persistence; // IUnitOfWork için
using HrApp.EmployeeManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.EmployeeManagement.Application.Features.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand>
{
    private readonly IUnitOfWork _unitOfWork; // DEĞİŞTİ
    private readonly IMapper _mapper; // Kullanılmıyor
    private readonly ILogger<UpdateDepartmentCommandHandler> _logger;

    public UpdateDepartmentCommandHandler(
        IUnitOfWork unitOfWork, // DEĞİŞTİ
        IMapper mapper,
        ILogger<UpdateDepartmentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork; // DEĞİŞTİ
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
         _logger.LogInformation("Attempting to update department with ID: {DepartmentId}", request.Id);
        var departmentToUpdate = await _unitOfWork.DepartmentRepository.GetByIdAsync(request.Id, cancellationToken); // UoW Kullanıldı

        if (departmentToUpdate == null)
        {
            _logger.LogWarning("Department with ID: {DepartmentId} not found for update.", request.Id);
            throw new NotFoundException(nameof(Department), request.Id);
        }

        departmentToUpdate.UpdateName(request.Name);
        _unitOfWork.DepartmentRepository.Update(departmentToUpdate); // UoW Kullanıldı

        await _unitOfWork.SaveChangesAsync(cancellationToken); // UoW Kullanıldı
        _logger.LogInformation("Database changes saved for updated department {DepartmentId}.", request.Id);

        _logger.LogInformation("Department with ID: {DepartmentId} updated successfully.", request.Id);
        // return Unit.Value;
    }
}