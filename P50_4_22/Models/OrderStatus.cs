using System;
using System.Collections.Generic;

namespace P50_4_22.Models;

public partial class OrderStatus
{
    public int IdStatus { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
