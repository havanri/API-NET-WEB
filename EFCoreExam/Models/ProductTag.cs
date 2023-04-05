using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreExam.Models
{
    public class ProductTag
    {
        public DateTime PublicationDate { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
