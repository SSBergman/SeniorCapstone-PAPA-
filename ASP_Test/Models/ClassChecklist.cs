using System;
using System.Collections.Generic;

namespace ASP_Test.Models;

public partial class ClassChecklist
{
    public string StudentId { get; set; } = null!;

    public string ClassId { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string CourseCode { get; set; } = null!;

    public string ClassName { get; set; } = null!;
    public string Status { get; set; } = null!;

    public bool? StatusBit { get; set; }

}
