using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreExam.Models
{
    public class Tag : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductTag> ProductTags { get; set; }
    }
}
