using AutoMapper;
using HrApp.EmployeeManagement.Application.Features.Employees.Dtos;
using HrApp.EmployeeManagement.Application.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace HrApp.EmployeeManagement.Application.Features.Employees.Queries.GetAllEmployees;

public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IReadOnlyList<EmployeeListVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper; // AutoMapper'ı tekrar kullanalım
    private readonly ILogger<GetAllEmployeesQueryHandler> _logger;

    public GetAllEmployeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllEmployeesQueryHandler> logger) // IMapper eklendi
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper; // Eklendi
        _logger = logger;
   }

    public async Task<IReadOnlyList<EmployeeListVm>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAllEmployeesQuery");
        // Departmanları include eden metodu çağır
        var employees = await _unitOfWork.EmployeeRepository.GetAllWithDepartmentsAsync(cancellationToken);
        _logger.LogInformation("Successfully retrieved {EmployeeCount} employees from repository.", employees.Count);

        // --- DEĞİŞİKLİK: AutoMapper ile Mapping ---
        var employeeDtos = _mapper.Map<IReadOnlyList<EmployeeListVm>>(employees);
        // --- BİTİŞ ---

        _logger.LogInformation("Successfully mapped {EmployeeCount} employees to EmployeeListVm.", employeeDtos.Count);
        return employeeDtos;
    }
}