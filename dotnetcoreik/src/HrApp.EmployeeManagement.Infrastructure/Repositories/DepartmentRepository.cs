// HrApp.EmployeeManagement.Infrastructure/Repositories/DepartmentRepository.cs

using HrApp.EmployeeManagement.Application.Persistence;
using HrApp.EmployeeManagement.Domain.Entities;
using HrApp.EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HrApp.EmployeeManagement.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly EmployeeManagementDbContext _dbContext;

    public DepartmentRepository(EmployeeManagementDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Departments.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Department?> GetFirstOrDefaultAsync(Expression<Func<Department, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Departments.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IReadOnlyList<Department>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Departments
                             // Departmanları listelerken ilişkili çalışanları çekmek istemeyiz (performans)
                             // .Include(d => d.Employees) // Gerekirse bu şekilde eklenir (explicit loading)
                             .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Department>> GetWhereAsync(Expression<Func<Department, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Departments.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Department entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Departments.AddAsync(entity, cancellationToken);
        // SaveChangesAsync is called elsewhere (e.g., in the service or controller)
    }

    public void Update(Department entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        // SaveChangesAsync is called elsewhere
    }

    public void Delete(Department entity)
    {
        // Dikkat: Departmanı silmeden önce ilişkili çalışanların
        // DepartmentId'sinin null yapılması (OnDelete SetNull) önemlidir.
        // Ancak yine de bu departmana bağlı başka kritik veriler olabilir.
        // Gerçek uygulamada "soft delete" (IsDeleted flag'i) daha güvenli olabilir.
         var employeesInDepartment = _dbContext.Employees.Any(e => e.DepartmentId == entity.Id);
         if (employeesInDepartment && _dbContext.Entry(entity).Metadata.FindNavigation(nameof(Department.Employees))?.ForeignKey.DeleteBehavior != DeleteBehavior.SetNull)
         {
              // Güvenlik önlemi olarak, eğer SetNull ayarlanmamışsa ve çalışan varsa silmeyi engelle (veya logla/hata ver)
              throw new InvalidOperationException($"Cannot delete department '{entity.Name}' (ID: {entity.Id}) because it has associated employees and the delete behavior is not SetNull.");
         }

        _dbContext.Departments.Remove(entity);
        // SaveChangesAsync is called elsewhere
    }

     // Örnek: Departmanı çalışanlarıyla birlikte getirme (Explicit Loading)
    // public async Task<Department?> GetDepartmentWithEmployeesAsync(Guid id, CancellationToken cancellationToken = default)
    // {
    //     return await _dbContext.Departments
    //                          .Include(d => d.Employees) // İlişkili çalışanları da sorguya dahil et
    //                          .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    // }
}