using HrApp.EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HrApp.EmployeeManagement.Infrastructure.Data;

public class EmployeeManagementDbContext : DbContext
{
    public EmployeeManagementDbContext(DbContextOptions<EmployeeManagementDbContext> options)
        : base(options)
    {
    }

    // --- DbSets ---
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; } // Yeni DbSet eklendi
    // --- Bitiş: DbSets ---

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Employee yapılandırması
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.LastName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Email)
                  .IsRequired()
                  .HasMaxLength(150);
            entity.HasIndex(e => e.Email)
                  .IsUnique();

            entity.Property(e => e.Salary)
                  .HasColumnType("decimal(18,2)");

            entity.Property(e => e.Position).HasMaxLength(100);
            // entity.Property(e => e.Department).HasMaxLength(100); // Bu alanı kaldırdık

             entity.Property(e => e.DateOfBirth).IsRequired();
             entity.Property(e => e.HireDate).IsRequired();

            // --- Yeni İlişki Yapılandırması Eklendi ---
            entity.HasOne(e => e.Department) // Employee'nin bir Department'ı vardır
                  .WithMany(d => d.Employees) // Department'ın çok sayıda Employee'si vardır
                  .HasForeignKey(e => e.DepartmentId) // Employee tarafındaki Foreign Key 'DepartmentId'dir
                  .IsRequired(false) // Foreign key'in null olabileceğini belirtir (çünkü DepartmentId nullable)
                  .OnDelete(DeleteBehavior.SetNull); // Departman silinirse, ilişkili çalışanların DepartmentId'si NULL yapılsın.
             // --- Bitiş: Yeni İlişki Yapılandırması ---
        });

        // --- Department Yapılandırması Eklendi ---
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasIndex(d => d.Name) // Departman adının da benzersiz olmasını isteyebiliriz.
                  .IsUnique();

            // İlişkinin diğer ucu (Department -> Employees) EF Core tarafından genellikle
            // otomatik olarak yapılandırılır (WithMany sayesinde), ama istersen burada da belirtebilirsin:
            // entity.HasMany(d => d.Employees)
            //       .WithOne(e => e.Department)
            //       .HasForeignKey(e => e.DepartmentId); // Bu zaten Employee tarafında tanımlandı.
        });
         // --- Bitiş: Department Yapılandırması ---
    }
}