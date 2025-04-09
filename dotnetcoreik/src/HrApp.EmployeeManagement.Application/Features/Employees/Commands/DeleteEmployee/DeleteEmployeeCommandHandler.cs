// src/HrApp.EmployeeManagement.Application/Features/Employees/Commands/DeleteEmployee/DeleteEmployeeCommandHandler.cs
using HrApp.EmployeeManagement.Application.Exceptions;
using HrApp.EmployeeManagement.Application.Persistence; // IUnitOfWork için
using HrApp.EmployeeManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HrApp.EmployeeManagement.Application.Features.Employees.Commands.DeleteEmployee;

 public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
 {
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteEmployeeCommandHandler> _logger;

    public DeleteEmployeeCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteEmployeeCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to delete employee with ID: {EmployeeId}", request.Id);
        var employeeToDelete = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.Id, cancellationToken);

        if (employeeToDelete == null)
        {
             _logger.LogWarning("Employee with ID: {EmployeeId} not found for deletion.", request.Id);
            throw new NotFoundException(nameof(Employee), request.Id);
        }

        _unitOfWork.EmployeeRepository.Delete(employeeToDelete);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Database changes saved for deleted employee {EmployeeId}.", request.Id);

        _logger.LogInformation("Employee with ID: {EmployeeId} successfully deleted.", request.Id);
        // return Unit.Value;
    }
 }