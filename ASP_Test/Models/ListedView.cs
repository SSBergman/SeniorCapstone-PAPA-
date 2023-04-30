using System;
using System.Collections.Generic;

namespace ASP_Test.Models;

public partial class ListedView
{
    public string CourseCode { get; set; } = null!;

    public string ClassName { get; set; } = null!;

    public string? Prerequisites { get; set; }

    public string ListId { get; set; } = null!;

    public string? SemesterId { get; set; }

    public string? ClassId { get; set; }
}
