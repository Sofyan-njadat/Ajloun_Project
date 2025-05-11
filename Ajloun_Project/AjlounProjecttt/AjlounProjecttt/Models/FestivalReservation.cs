using System;
using System.Collections.Generic;

namespace AjlounProjecttt.Models;

public partial class FestivalReservation
{
    public int ReservationId { get; set; }

    public int? FestivalId { get; set; }

    public int? UserId { get; set; }

    public DateTime? ReservationDate { get; set; }

    public DateOnly AttendanceDate { get; set; }

    public string? Status { get; set; }

    public virtual Festival? Festival { get; set; }

    public virtual User? User { get; set; }
}
