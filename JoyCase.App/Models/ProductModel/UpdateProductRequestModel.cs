namespace JoyCase.App.Models.ProductModel
{
    public class UpdateProductRequestModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public long CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
        public string UpdatedBy { get; set; } = null!;
    }
}
