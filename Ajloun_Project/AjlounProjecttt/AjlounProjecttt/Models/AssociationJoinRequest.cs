using System;
using System.Collections.Generic;

namespace AjlounProjecttt.Models;

public partial class AssociationJoinRequest
{
    public int RequestId { get; set; }

    public int? AssociationId { get; set; }

    public int? UserId { get; set; }

    public string? Status { get; set; }

    public virtual CulturalAssociation? Association { get; set; }

    public virtual User? User { get; set; }
}
