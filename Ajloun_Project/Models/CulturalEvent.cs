using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class CulturalEvent
{
    public int EventId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? EventType { get; set; }

    public DateTime? Date { get; set; }

    public string? Location { get; set; }

    public string? PosterUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public TimeOnly? Time { get; set; }

    public virtual ICollection<CulturalEventReservation> CulturalEventReservations { get; set; } = new List<CulturalEventReservation>();
}
