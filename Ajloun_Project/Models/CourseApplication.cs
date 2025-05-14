using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class CourseApplication
{
    public int ApplicationId { get; set; }

    public int CourseId { get; set; }

    public string? BirthCertificateImage { get; set; }

    public bool Agreement { get; set; }

    public string? Status { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public int? UserId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User? User { get; set; }
}
