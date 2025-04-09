// src/HrApp.EmployeeManagement.Application/Features/Employees/Commands/UpdateEmployee/UpdateEmployeeCommandHandler.cs

using AutoMapper;
using HrApp.EmployeeManagement.Application.Exceptions;
using HrApp.EmployeeManagement.Application.Persistence; // IUnitOfWork için
using HrApp.EmployeeManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.EmployeeManagement.Application.Features.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper; // Kullanılmıyor ama kalsın
    private readonly ILogger<UpdateEmployeeCommandHandler> _logger;

    public UpdateEmployeeCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateEmployeeCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to update employee with ID: {EmployeeId}", request.Id);
        var employeeToUpdate = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.Id, cancellationToken);

        if (employeeToUpdate == null)
        {
            _logger.LogWarning("Employee with ID: {EmployeeId} not found for update.", request.Id);
            throw new NotFoundException(nameof(Employee), request.Id);
        }

        if (request.DepartmentId.HasValue && request.DepartmentId != Guid.Empty)
        {
            var departmentExists = await _unitOfWork.DepartmentRepository.GetByIdAsync(request.DepartmentId.Value, cancellationToken);
            if (departmentExists == null)
            {
                 _logger.LogWarning("Attempted to update employee {EmployeeId} with non-existent Department ID: {DepartmentId}", request.Id, request.DepartmentId.Value);
                throw new BadRequestException($"Invalid Department ID: {request.DepartmentId.Value}");
            }
        }

        // --- Tarihi UTC'ye Çevir ---
        var dateOfBirthUtc = DateTime.SpecifyKind(request.DateOfBirth, DateTimeKind.Utc);
        // --- Bitiş ---

        employeeToUpdate.UpdatePersonalInfo(request.FirstName, request.LastName, request.Email, dateOfBirthUtc); // UTC Kullan
        employeeToUpdate.UpdatePosition(request.Position);
        employeeToUpdate.AssignDepartment(request.DepartmentId);
        if (request.Salary.HasValue) { employeeToUpdate.UpdateSalary(request.Salary.Value); }

        _unitOfWork.EmployeeRepository.Update(employeeToUpdate);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Database changes saved for updated employee {EmployeeId}.", request.Id);

        _logger.LogInformation("Employee with ID: {EmployeeId} updated successfully.", request.Id);
        // return Unit.Value;
    }
}