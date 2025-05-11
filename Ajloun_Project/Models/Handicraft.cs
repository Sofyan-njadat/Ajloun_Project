using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class Handicraft
{
    public int CraftId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? Stock { get; set; }

    public string? Category { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<CraftOrder> CraftOrders { get; set; } = new List<CraftOrder>();
}
