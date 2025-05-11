using System;
using System.Collections.Generic;

namespace AjlounProjecttt.Models;

public partial class AssociationEvent
{
    public int AssocEventId { get; set; }

    public int? AssociationId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public string? Location { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<AssocEventRegistration> AssocEventRegistrations { get; set; } = new List<AssocEventRegistration>();

    public virtual CulturalAssociation? Association { get; set; }
}
