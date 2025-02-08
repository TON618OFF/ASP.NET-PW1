using System;
using System.Collections.Generic;

namespace P50_4_22.Models;

public partial class Review
{
    public int IdReview { get; set; }

    public int ProductId { get; set; }

    public int ClientNameId { get; set; }

    public int ReviewRating { get; set; }

    public string ReviewComment { get; set; } = null!;

    public DateTime ReviewCreatedDate { get; set; }

    public virtual Client ClientName { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
