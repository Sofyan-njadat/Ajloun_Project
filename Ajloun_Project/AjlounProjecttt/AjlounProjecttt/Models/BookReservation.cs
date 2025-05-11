using System;
using System.Collections.Generic;

namespace AjlounProjecttt.Models;

public partial class BookReservation
{
    public int ReservationId { get; set; }

    public int BookId { get; set; }

    public int? UserId { get; set; }

    public DateTime? ReservationDate { get; set; }

    public DateOnly? PickupDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public string Status { get; set; } = null!;

    public bool? Agreement { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User? User { get; set; }
}
