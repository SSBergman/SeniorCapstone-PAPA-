using System;
using System.Collections.Generic;

namespace ASP_Test.Models;

public partial class Student
{
    public int IdNum { get; set; }

    public string StudentId { get; set; } = null!;

    public string? DegreeId { get; set; }

    public string Email { get; set; } = null!;

    public string StudentPassword { get; set; } = null!;

    public string? GradYear { get; set; }

    public virtual ICollection<CompletedClass> CompletedClasses { get; } = new List<CompletedClass>();

    public virtual Degree? Degree { get; set; }

    public virtual ICollection<Semester> Semesters { get; } = new List<Semester>();
}
