using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class Festival
{
    public int FestivalId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? FestivalType { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Location { get; set; }

    public string? BannerImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<FestivalReservation> FestivalReservations { get; set; } = new List<FestivalReservation>();
}
