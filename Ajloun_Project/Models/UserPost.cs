using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class UserPost
{
    public int PostId { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } = null!;

    public string? Category { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string FilePath { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
