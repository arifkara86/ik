using System;

namespace HrApp.EmployeeManagement.Domain.Common
{
    // BaseEntity'den miras alarak ID özelliğini de içerir.
    public abstract class BaseAuditableEntity : BaseEntity
    {
        // Kaydın oluşturulma tarihi
        public DateTime CreatedDate { get; set; }

        // Kaydı oluşturan kullanıcı (Kimlik doğrulama entegre edildiğinde doldurulacak)
        public string? CreatedBy { get; set; } // Null olabilir başlangıçta

        // Kaydın son güncellenme tarihi
        public DateTime? LastModifiedDate { get; set; } // Null olabilir

        // Kaydı son güncelleyen kullanıcı
        public string? LastModifiedBy { get; set; } // Null olabilir
    }
}