using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class Artwork
{
    public int ArtworkId { get; set; }

    public string? Title { get; set; }

    public string? ArtistName { get; set; }

    public string? ArtistEmail { get; set; }

    public string? PortfolioUrl { get; set; }

    public string? Type { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }
}
