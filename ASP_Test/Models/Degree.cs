using System;
using System.Collections.Generic;

namespace ASP_Test.Models;

public partial class Degree
{
    public int IdNum { get; set; }

    public string DegreeId { get; set; } = null!;

    public string DegreeName { get; set; } = null!;

    public string VersionYear { get; set; } = null!;

    public virtual ICollection<Requirement> Requirements { get; } = new List<Requirement>();

    public virtual ICollection<Student> Students { get; } = new List<Student>();
}
