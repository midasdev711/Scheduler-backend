using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Models
{
    public partial class WorkingHours
    {
        public WorkingHours()
        {
        }

        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
