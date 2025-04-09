// src/HrApp.EmployeeManagement.Application/Persistence/IUnitOfWork.cs
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HrApp.EmployeeManagement.Application.Persistence;

// IDisposable eklemek, DbContext'in dispose edilmesini sağlamak için iyi bir pratiktir.
public interface IUnitOfWork : IDisposable
{
    // Repository'lere erişim (sadece get)
    IEmployeeRepository EmployeeRepository { get; }
    IDepartmentRepository DepartmentRepository { get; }

    // Değişiklikleri kaydetme metodu
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}