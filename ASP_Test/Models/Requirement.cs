using System;
using System.Collections.Generic;

namespace ASP_Test.Models;

public partial class Requirement
{
    public int IdNum { get; set; }

    public string ReqId { get; set; } = null!;

    public string DegreeId { get; set; } = null!;

    public string ClassId { get; set; } = null!;

    public virtual Class Class { get; set; } = null!;

    public virtual Degree Degree { get; set; } = null!;
}
