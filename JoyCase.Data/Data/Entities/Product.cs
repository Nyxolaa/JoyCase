namespace JoyCase.Data;

public partial class Product
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long CategoryId { get; set; }

    public string? ImageUrl { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? DeletedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Category Category { get; set; } = null!;
}
