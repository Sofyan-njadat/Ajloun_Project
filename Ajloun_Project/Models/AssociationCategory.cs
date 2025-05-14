using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class AssociationCategory
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CulturalAssociation> CulturalAssociations { get; set; } = new List<CulturalAssociation>();
}
