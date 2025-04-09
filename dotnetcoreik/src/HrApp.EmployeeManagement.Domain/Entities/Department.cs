using System.Collections.Generic; // ICollection için eklendi

namespace HrApp.EmployeeManagement.Domain.Entities;

public class Department
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    // Navigation Property: Bu departmana ait çalışanların listesi
    public virtual ICollection<Employee> Employees { get; private set; } = new List<Employee>();

    // Constructor
    public Department(Guid id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Department name cannot be empty.", nameof(name));
        }
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Name = name;
    }

    // Davranış Metodu (Örnek)
    public void UpdateName(string newName)
    {
         if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("Department name cannot be empty.", nameof(newName));
        }
        Name = newName;
    }

    // EF Core için korumalı parametresiz constructor
    protected Department() { }
}