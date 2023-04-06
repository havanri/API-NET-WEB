namespace EFCoreExam.DTOs.Category
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
        public IFormFile formFile { get; set; }
    }
}
