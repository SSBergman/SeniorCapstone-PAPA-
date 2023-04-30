using System;
using System.Collections.Generic;

namespace ASP_Test.Models;

public partial class RequiredForDegree
{
    public string DegreeId { get; set; } = null!;

    public string ClassId { get; set; } = null!;

    public string CourseCode { get; set; } = null!;

    public string ClassName { get; set; } = null!;
    public string RequiredBit { get; set; } = null;

}
