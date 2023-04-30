using System;
using System.Collections.Generic;

namespace ASP_Test.Models;

public partial class Class
{
    public int IdNum { get; set; }

    public string ClassId { get; set; } = null!;

    public string CourseCode { get; set; } = null!;

    public string ClassName { get; set; } = null!;

    public string Category { get; set; } = null!;
    public string? Prerequisites { get; set; }

    public bool? InFall { get; set; }

    public bool? InSpring { get; set; }

    public bool? InSummer { get; set; }

    public bool? IsOffered { get; set; }

    public virtual ICollection<CompletedClass> CompletedClasses { get; } = new List<CompletedClass>();

    public virtual ICollection<ListedClass> ListedClasses { get; } = new List<ListedClass>();

    public virtual ICollection<Requirement> Requirements { get; } = new List<Requirement>();
}
