using System;
using System.Collections.Generic;

namespace P50_4_22.Models;

public partial class CartItem
{
    public int IdCartItem { get; set; }

    public int ClientId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
