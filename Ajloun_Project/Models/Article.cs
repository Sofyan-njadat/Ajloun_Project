using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class Article
{
    public int ArticleId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? Category { get; set; }

    public string? AuthorName { get; set; }

    public DateTime? PublishDate { get; set; }

    public string? CoverImageUrl { get; set; }
}
