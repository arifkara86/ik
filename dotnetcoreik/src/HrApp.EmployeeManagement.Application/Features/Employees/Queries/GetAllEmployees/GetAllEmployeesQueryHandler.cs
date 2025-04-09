using AutoMapper;
using HrApp.EmployeeManagement.Application.Features.Employees.Dtos;
using HrApp.EmployeeManagement.Application.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
namespace HrApp.EmployeeManagement.Application.Features.Employees.Queries.GetAllEmployees;
public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IReadOnlyList<EmployeeListVm>> {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllEmployeesQueryHandler> _logger;
    public GetAllEmployeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllEmployeesQueryHandler> logger){
        _unitOfWork = unitOfWork; _mapper = mapper; _logger = logger;
    }
    public async Task<IReadOnlyList<EmployeeListVm>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken){
        _logger.LogInformation("Handling GetAllEmployeesQuery");
        var employees = await _unitOfWork.EmployeeRepository.GetAllWithDepartmentsAsync(cancellationToken); // Doğru metot çağrılıyor
        _logger.LogInformation("Successfully retrieved {EmployeeCount} employees from repository.", employees.Count);
        var employeeDtos = _mapper.Map<IReadOnlyList<EmployeeListVm>>(employees);
        _logger.LogInformation("Successfully mapped {EmployeeCount} employees to EmployeeListVm.", employeeDtos.Count);
        return employeeDtos;
    }
}