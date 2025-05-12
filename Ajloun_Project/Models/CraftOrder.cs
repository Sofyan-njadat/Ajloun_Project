using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class CraftOrder
{
    public int OrderId { get; set; }

    public int? CraftId { get; set; }

    public int? UserId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? Status { get; set; }

    public virtual Handicraft? Craft { get; set; }

    public virtual User? User { get; set; }
}
