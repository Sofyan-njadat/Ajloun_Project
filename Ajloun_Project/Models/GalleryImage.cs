using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class GalleryImage
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string Category { get; set; } = null!;
}
