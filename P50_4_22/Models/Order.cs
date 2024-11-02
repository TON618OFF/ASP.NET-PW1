using System;
using System.Collections.Generic;

namespace P50_4_22.Models;

public partial class Order
{
    public int IdOrder { get; set; }

    public string OrderNumber { get; set; } = null!;

    public string OrderDate { get; set; } = null!;

    public decimal OrderTotalSum { get; set; }

    public int StatusId { get; set; }

    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<PositionOrder> PositionOrders { get; set; } = new List<PositionOrder>();

    public virtual OrderStatus Status { get; set; } = null!;
}
