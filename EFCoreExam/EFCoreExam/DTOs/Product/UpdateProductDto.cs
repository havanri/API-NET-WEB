using Microsoft.AspNetCore.Http;

namespace EFCoreExam
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ShortDescription { get; set; }
        public string MainDescription { get; set; }
        public IFormFile? formFile { get; set; }
        public int CategoryId { get; set; }
    }
}
