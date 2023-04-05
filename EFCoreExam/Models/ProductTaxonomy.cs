using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreExam.Models
{
    public class ProductTaxonomy
    {
        public DateTime PublicationDate { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }


        public int TaxonomyId { get; set; }
        public Taxonomy? Taxonomy { get; set; }
    }
}
