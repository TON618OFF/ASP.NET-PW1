using System;
using System.Collections.Generic;

namespace P50_4_22.Models;

public partial class Product
{
    public int IdProduct { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductImage { get; set; } = null!;

    public decimal ProductPrice { get; set; }

    public string ProductDescription { get; set; } = null!;

    public int ProductAmount { get; set; }

    public int CategoryId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<PositionOrder> PositionOrders { get; set; } = new List<PositionOrder>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
