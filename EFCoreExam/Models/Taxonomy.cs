using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreExam.Models
{
    public class Taxonomy : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<ProductTaxonomy> ProductTaxonomies { get; set; }

        public int AttributeId { get; set; }
        public Attribute Attribute { get; set; }
    }
}
