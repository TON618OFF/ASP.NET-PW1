using System;
using System.Collections.Generic;

namespace P50_4_22.Models;

public partial class Category
{
    public int IdCategory { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
