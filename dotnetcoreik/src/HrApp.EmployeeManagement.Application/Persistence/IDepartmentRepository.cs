// HrApp.EmployeeManagement.Application/Persistence/IDepartmentRepository.cs

using HrApp.EmployeeManagement.Domain.Entities;
using System.Linq.Expressions;

namespace HrApp.EmployeeManagement.Application.Persistence;

// EmployeeRepository'ye benzer temel CRUD işlemleri
public interface IDepartmentRepository
{
    Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Department?> GetFirstOrDefaultAsync(Expression<Func<Department, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Department>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Department>> GetWhereAsync(Expression<Func<Department, bool>> predicate, CancellationToken cancellationToken = default);
    Task AddAsync(Department entity, CancellationToken cancellationToken = default);
    void Update(Department entity);
    void Delete(Department entity);
    // İleride: Departmana ait çalışanları getirme gibi özel metotlar eklenebilir.
    // Task<Department?> GetDepartmentWithEmployeesAsync(Guid id, CancellationToken cancellationToken = default);
}