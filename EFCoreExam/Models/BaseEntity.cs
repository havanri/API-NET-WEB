using System.ComponentModel.DataAnnotations;

namespace EFCoreExam.Models
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
