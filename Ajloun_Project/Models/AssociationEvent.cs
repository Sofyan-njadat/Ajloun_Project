using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class AssociationEvent
{
    public int EventId { get; set; }

    public int? AssociationId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public string? Location { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? PosterUrl { get; set; }

    public string? EventType { get; set; }

    public virtual ICollection<AssocEventRegistration> AssocEventRegistrations { get; set; } = new List<AssocEventRegistration>();

    public virtual CulturalAssociation? Association { get; set; }
}
