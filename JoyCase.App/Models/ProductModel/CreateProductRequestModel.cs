namespace JoyCase.App.Models.ProductModel
{
    public class CreateProductRequestModel
    {
        public string Name { get; set; } = null!;
        public long CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; } = null!;
    }
}
