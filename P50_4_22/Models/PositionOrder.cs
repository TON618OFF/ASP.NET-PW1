using System;
using System.Collections.Generic;

namespace P50_4_22.Models;

public partial class PositionOrder
{
    public int IdPositionOrder { get; set; }

    public int PositionOrderCount { get; set; }

    public decimal PositionOrderPrice { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
