using System;
using System.Collections.Generic;

namespace P50_4_22.Models;

public partial class ClientsAddress
{
    public int IdClientAddress { get; set; }

    public string AddressLocation { get; set; } = null!;

    public string AddressCity { get; set; } = null!;

    public string AddressPostalCode { get; set; } = null!;

    public string AddressCountry { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
