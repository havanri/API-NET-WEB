namespace EFCoreExam.Response
{
    public class ProductWithCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? ShortDescription { get; set; }
        public string? MainDescription { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
