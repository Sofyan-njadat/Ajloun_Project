using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class WorkshopRegistration
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int WorkshopId { get; set; }

    public string? BirthCertificateImage { get; set; }

    public string Status { get; set; } = null!;

    public string? RejectionReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ArtWorkshop Workshop { get; set; } = null!;
}
