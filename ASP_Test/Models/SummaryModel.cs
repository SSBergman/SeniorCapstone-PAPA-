using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ASP_Test.Models
{
    public partial class SummaryModel
    {
        public List<string> result { get; set; }=new List<string>();
        public List<int> format { get; set; } =new List<int>();

    }
}
