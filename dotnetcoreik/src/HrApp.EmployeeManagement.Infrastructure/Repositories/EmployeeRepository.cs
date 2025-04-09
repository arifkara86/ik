using HrApp.EmployeeManagement.Application.Persistence;
using HrApp.EmployeeManagement.Domain.Entities;
using HrApp.EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace HrApp.EmployeeManagement.Infrastructure.Repositories;
public class EmployeeRepository : IEmployeeRepository {
    private readonly EmployeeManagementDbContext _dbContext;
    public EmployeeRepository(EmployeeManagementDbContext dbContext){ _dbContext = dbContext; }
    public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default){
        return await _dbContext.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
    public async Task<Employee?> GetFirstOrDefaultAsync(Expression<Func<Employee, bool>> predicate, CancellationToken cancellationToken = default){
        return await _dbContext.Employees.Include(e => e.Department).FirstOrDefaultAsync(predicate, cancellationToken);
    }
    public async Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken = default){
        return await _dbContext.Employees.ToListAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<Employee>> GetAllWithDepartmentsAsync(CancellationToken cancellationToken = default){
        return await _dbContext.Employees.Include(e => e.Department).AsNoTracking().ToListAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<Employee>> GetWhereAsync(Expression<Func<Employee, bool>> predicate, CancellationToken cancellationToken = default){
        return await _dbContext.Employees.Include(e => e.Department).Where(predicate).AsNoTracking().ToListAsync(cancellationToken);
    }
    public async Task AddAsync(Employee entity, CancellationToken cancellationToken = default){
        await _dbContext.Employees.AddAsync(entity, cancellationToken);
    }
    public void Update(Employee entity){ _dbContext.Entry(entity).State = EntityState.Modified; }
    public void Delete(Employee entity){ _dbContext.Employees.Remove(entity); }
}