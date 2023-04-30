using System;
using System.Collections.Generic;

namespace ASP_Test.Models;

public partial class CompletedClass
{
    public int IdNum { get; set; }

    public string CompleteId { get; set; } = null!;

    public string? StudentId { get; set; }

    public string? ClassId { get; set; }

    public virtual Class? Class { get; set; }

    public virtual Student? Student { get; set; }
}
