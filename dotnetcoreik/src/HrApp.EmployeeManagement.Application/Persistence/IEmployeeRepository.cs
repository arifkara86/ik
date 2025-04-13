using HrApp.EmployeeManagement.Domain.Entities;
using System.Linq.Expressions;

namespace HrApp.EmployeeManagement.Application.Persistence;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Employee?> GetByIdWithDepartmentAsync(Guid id, CancellationToken cancellationToken = default); // Edit için eklendi
    Task<Employee?> GetFirstOrDefaultAsync(Expression<Func<Employee, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Employee>> GetWhereAsync(Expression<Func<Employee, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Employee>> GetAllWithDepartmentsAsync(CancellationToken cancellationToken = default); // Eklendiğinden emin ol
    Task AddAsync(Employee entity, CancellationToken cancellationToken = default);
    void Update(Employee entity);
    void Delete(Employee entity);
}