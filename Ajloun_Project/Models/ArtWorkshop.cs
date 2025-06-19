using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class ArtWorkshop
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Category { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int? MinAge { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsHidden { get; set; }

    public int? MaxAge { get; set; }

    public virtual ICollection<WorkshopRegistration> WorkshopRegistrations { get; set; } = new List<WorkshopRegistration>();
}
