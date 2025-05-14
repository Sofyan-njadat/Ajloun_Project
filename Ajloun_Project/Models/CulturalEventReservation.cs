using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class CulturalEventReservation
{
    public int ReservationId { get; set; }

    public int? EventId { get; set; }

    public DateTime? ReservationDate { get; set; }

    public string? Status { get; set; }

    public int? UserId { get; set; }

    public virtual CulturalEvent? Event { get; set; }

    public virtual User? User { get; set; }
}
