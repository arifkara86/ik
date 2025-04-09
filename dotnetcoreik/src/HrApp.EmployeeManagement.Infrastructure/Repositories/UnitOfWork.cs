// src/HrApp.EmployeeManagement.Infrastructure/Repositories/UnitOfWork.cs
using HrApp.EmployeeManagement.Application.Persistence;
using HrApp.EmployeeManagement.Infrastructure.Data; // DbContext için
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HrApp.EmployeeManagement.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly EmployeeManagementDbContext _dbContext;
    private IEmployeeRepository? _employeeRepository; // Lazy loading için nullable
    private IDepartmentRepository? _departmentRepository; // Lazy loading için nullable

    public UnitOfWork(EmployeeManagementDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    // Repository'ler istendiğinde oluşturulur (Lazy Loading)
    public IEmployeeRepository EmployeeRepository =>
        _employeeRepository ??= new EmployeeRepository(_dbContext);

    public IDepartmentRepository DepartmentRepository =>
        _departmentRepository ??= new DepartmentRepository(_dbContext);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    // IDisposable implementasyonu
    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose(); // DbContext'i dispose et
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}