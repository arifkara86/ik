using HrApp.EmployeeManagement.Domain.Common; // BaseAuditableEntity için
using System; // DateTime için

namespace HrApp.EmployeeManagement.Domain.Entities
{
    // Employee entity'si de denetlenebilir alanları içersin
    public class Employee : BaseAuditableEntity
    {
        // Temel Personel Bilgileri
        public string FirstName { get; set; } = string.Empty; // Boş string ile başlatmak null riskini azaltır
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; // Unique olmalı (DB seviyesinde ayarlanacak)
        public string? PhoneNumber { get; set; } // Opsiyonel
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; } // İşe Giriş Tarihi

        // Departman İlişkisi (Foreign Key ve Navigation Property)
        public Guid DepartmentId { get; set; } // Foreign Key
        public virtual Department Department { get; set; } = null!; // Navigation Property (null olamaz varsayımı)

        // Pozisyon İlişkisi (Foreign Key ve Navigation Property)
        public Guid PositionId { get; set; } // Foreign Key
        public virtual Position Position { get; set; } = null!; // Navigation Property (null olamaz varsayımı)

        // Diğer olası alanlar (Adres, Maaş Bilgisi - ayrı bir entity olabilir vb.)
        // public Address Address { get; set; }
        // public decimal Salary { get; set; }

        // EF Core için gerekli olabilecek parametresiz constructor
        protected Employee() { }

        // Yeni bir çalışan oluşturmak için kullanılabilecek örnek bir constructor
        // Daha fazla zorunlu alan varsa buraya eklenebilir.
        public Employee(string firstName, string lastName, string email, DateTime dateOfBirth, DateTime hireDate, Guid departmentId, Guid positionId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email; // Email validasyonu Application katmanında yapılmalı
            DateOfBirth = dateOfBirth;
            HireDate = hireDate;
            DepartmentId = departmentId;
            PositionId = positionId;

            // BaseAuditableEntity'den gelen Id otomatik oluşur
        }

        // Çalışanın departmanını güncellemek için metot (Örnek)
        public void ChangeDepartment(Guid newDepartmentId)
        {
            if (DepartmentId != newDepartmentId)
            {
                DepartmentId = newDepartmentId;
                // Domain Event tetiklenebilir
            }
        }
         // Çalışanın pozisyonunu güncellemek için metot (Örnek)
         public void ChangePosition(Guid newPositionId)
        {
            if (PositionId != newPositionId)
            {
                PositionId = newPositionId;
                // Domain Event tetiklenebilir
            }
        }
    }
}