using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string? Author { get; set; }

    public string? Description { get; set; }

    public string? CoverImageUrl { get; set; }

    public int? PublishedYear { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsAvailable { get; set; }

    public int AvailableCopies { get; set; }

    public int? CategoryId { get; set; }

    public virtual ICollection<BookReservation> BookReservations { get; set; } = new List<BookReservation>();

    public virtual BookCategory? Category { get; set; }
}
