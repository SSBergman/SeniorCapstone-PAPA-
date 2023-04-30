using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ASP_Test.Models
{
    public partial class SemPlanner
    {
        public List<ReqChecklist> Requirements { get; set; }

        public List<ListableClass> Classes { get; set; }
        public Semester Semesters { get; set; }

        public List<ListedView> Listed { get; set; }
        public int index { get; set; }
        public int count { get; set; }
    }
}
