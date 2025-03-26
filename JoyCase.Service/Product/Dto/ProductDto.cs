namespace JoyCase.Application.Product.Dto
{
    public class ProductDto
    {
        public long? ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public long? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public long? ParentId { get; set; }

    }
}
