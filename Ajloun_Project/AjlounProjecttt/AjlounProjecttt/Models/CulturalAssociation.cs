using System;
using System.Collections.Generic;

namespace AjlounProjecttt.Models;

public partial class CulturalAssociation
{
    public int AssociationId { get; set; }

    public string? Name { get; set; }

    public string? PresidentName { get; set; }

    public string? Phone { get; set; }

    public string? Region { get; set; }

    public int? FoundedYear { get; set; }

    public int? MembersCount { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<AssociationEvent> AssociationEvents { get; set; } = new List<AssociationEvent>();

    public virtual ICollection<AssociationJoinRequest> AssociationJoinRequests { get; set; } = new List<AssociationJoinRequest>();
}
