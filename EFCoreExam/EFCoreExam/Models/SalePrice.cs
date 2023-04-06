using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreExam.Models
{
    public class SalePrice
    {
        [Key]
        public int Id { get; set; }
        [Precision(10, 0)]
        public decimal Price { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
