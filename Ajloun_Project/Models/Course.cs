using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? AgeRange { get; set; }

    public string? Courseimg { get; set; }

    public bool? IsVisible { get; set; }

    public virtual ICollection<CourseApplication> CourseApplications { get; set; } = new List<CourseApplication>();
}
