using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;

namespace ASP_Test.Models;

public partial class DegreeForm
{ 
    public string DegreeName { get; set; } = null!;

    public string VersionYear { get; set; } = null!;
    public string DegreeId { get; set; } = null!;

    public List<ReqList> Requirements { get; set; } = new List<ReqList>();
}
