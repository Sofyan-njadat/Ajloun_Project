﻿using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class BookCategory
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
