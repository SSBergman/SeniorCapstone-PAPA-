using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ASP_Test.Models
{
    public partial class SettingsModel
    {
        public List<Degree> Degrees { get; set; }

        public string[] Years { get; set; }
        public string GradYear { get; set; }
        public string CurrentDegree { get; set; }
    }
}
