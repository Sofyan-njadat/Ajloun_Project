using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Address { get; set; }

    public string? NationalId { get; set; }

    public string? ProfileImage { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<AssocEventRegistration> AssocEventRegistrations { get; set; } = new List<AssocEventRegistration>();

    public virtual ICollection<AssociationJoinRequest> AssociationJoinRequests { get; set; } = new List<AssociationJoinRequest>();

    public virtual ICollection<BookReservation> BookReservations { get; set; } = new List<BookReservation>();

    public virtual ICollection<CourseApplication> CourseApplications { get; set; } = new List<CourseApplication>();

    public virtual ICollection<CraftOrder> CraftOrders { get; set; } = new List<CraftOrder>();

    public virtual ICollection<CulturalEventReservation> CulturalEventReservations { get; set; } = new List<CulturalEventReservation>();

    public virtual ICollection<FestivalReservation> FestivalReservations { get; set; } = new List<FestivalReservation>();
}
