// src/HrApp.EmployeeManagement.Application/Features/Employees/Commands/CreateEmployee/CreateEmployeeCommandHandler.cs

using AutoMapper;
using HrApp.EmployeeManagement.Application.Exceptions;
using HrApp.EmployeeManagement.Application.Persistence; // IUnitOfWork için
using HrApp.EmployeeManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper; // Kullanılmıyor ama kalsın
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;

    public CreateEmployeeCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateEmployeeCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        if (request.DepartmentId.HasValue && request.DepartmentId != Guid.Empty)
        {
            var departmentExists = await _unitOfWork.DepartmentRepository.GetByIdAsync(request.DepartmentId.Value, cancellationToken);
            if (departmentExists == null)
            {
                _logger.LogWarning("Attempted to create employee with non-existent Department ID: {DepartmentId}", request.DepartmentId.Value);
                throw new BadRequestException($"Invalid Department ID: {request.DepartmentId.Value}");
            }
        }

        // --- Tarihleri UTC'ye Çevir ---
        var dateOfBirthUtc = DateTime.SpecifyKind(request.DateOfBirth, DateTimeKind.Utc);
        var hireDateUtc = DateTime.SpecifyKind(request.HireDate, DateTimeKind.Utc);

        var employeeEntity = new Employee(
            Guid.Empty,
            request.FirstName,
            request.LastName,
            request.Email,
            dateOfBirthUtc, // UTC
            hireDateUtc     // UTC
        );
        // --- Bitiş ---

        employeeEntity.UpdatePosition(request.Position);
        employeeEntity.AssignDepartment(request.DepartmentId);
        if (request.Salary >= 0) { employeeEntity.UpdateSalary(request.Salary); }

        _logger.LogInformation("Adding new employee '{FirstName} {LastName}' to database.", request.FirstName, request.LastName);
        await _unitOfWork.EmployeeRepository.AddAsync(employeeEntity, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Database changes saved for new employee {EmployeeId}.", employeeEntity.Id);

        _logger.LogInformation("Employee '{FirstName} {LastName}' added successfully with ID {EmployeeId}.", request.FirstName, request.LastName, employeeEntity.Id);
        return employeeEntity.Id;
    }
}