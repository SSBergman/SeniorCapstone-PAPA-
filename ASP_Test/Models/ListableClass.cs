using System;
using System.Collections.Generic;

namespace ASP_Test.Models;

public partial class ListableClass
{
    public string? ClassId { get; set; } = null!;
    public string Category { get; set; } = null!;

    public string CourseCode { get; set; } = null!;

    public string ClassName { get; set; } = null!;

    public string? Prerequisites { get; set; }

    public string? Spring { get; set; }

    public string? Summer { get; set; }

    public string? Fall { get; set; }
    public string? Available { get; set; }
}
