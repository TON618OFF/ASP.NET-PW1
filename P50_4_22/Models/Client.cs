using System;
using System.Collections.Generic;

namespace P50_4_22.Models;

public partial class Client
{
    public int IdClient { get; set; }

    public string ClientLogin { get; set; } = null!;

    public string ClientPassword { get; set; } = null!;

    public string ClientSurname { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public string ClientMiddleName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int ClientAddressId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ClientsAddress ClientAddress { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
