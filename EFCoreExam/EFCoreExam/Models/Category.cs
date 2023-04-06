using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreExam.Models
{
    public class Category : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [DefaultValue(0)]
        public int ParentId { get; set; }
        public string? Slug { get; set; }
        public string? Thumbnail { get; set; }
        public string? ThumbnailPath { get; set; }
        public List<Product> Products { get; set; }
    }
}
