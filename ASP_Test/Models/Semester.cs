using System;
using System.Collections.Generic;

namespace ASP_Test.Models;

public partial class Semester
{
    public int IdNum { get; set; }

    public string SemesterId { get; set; } = null!;

    public string? StudentId { get; set; }

    public string Season { get; set; } = null!;

    public string SemYear { get; set; } = null!;

    public virtual ICollection<ListedClass> ListedClasses { get; } = new List<ListedClass>();

    public virtual Student? Student { get; set; }
}
