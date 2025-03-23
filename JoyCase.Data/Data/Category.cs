using System;
using System.Collections.Generic;

namespace JoyCase.Data.Data;

public partial class Category
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? ParentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? DeletedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual Category? Parent { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
