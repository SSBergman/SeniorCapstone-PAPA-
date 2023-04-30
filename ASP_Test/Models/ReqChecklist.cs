using System;
using System.Collections.Generic;

namespace ASP_Test.Models;

public partial class ReqChecklist
{
    public string DegreeName { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string CourseCode { get; set; } = null!;

    public string? Complete { get; set; }
}
