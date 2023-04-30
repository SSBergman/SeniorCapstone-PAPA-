using System;
using System.Collections.Generic;

namespace ASP_Test.Models;

public partial class AdminReqModel
{
    public string DegreeId { get; set; } = null!;
    public string DegreeName { get; set; } = null!;
    public string VersionYear { get; set; } = null!;

    public List<RequiredForDegree> Classes { get; set; }
}
