using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreExam
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 1000000, ErrorMessage = "Price must be between 0 and 1000000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Short description is required")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Main description is required")]
        public string MainDescription { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile formFile { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        public int CategoryId { get; set; }
    }
}

