using System;
using System.Collections.Generic;

namespace ASP_Test.Models;

public partial class ListedClass
{
    public int IdNum { get; set; }

    public string ListId { get; set; } = null!;

    public string? SemesterId { get; set; }

    public string? ClassId { get; set; }

    public virtual Class? Class { get; set; }

    public virtual Semester? Semester { get; set; }
}
