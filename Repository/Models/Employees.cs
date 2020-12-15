using System;
using System.Collections.Generic;

namespace Repository.Models
{
    public partial class Employees
    {
        public Employees()
        {
            ProjectRevisions = new HashSet<ProjectRevisions>();
            Events = new HashSet<Events>();
        }

        public int EmployeeId { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? DepartmentId { get; set; }

        public virtual Departments Department { get; set; }
        public virtual ICollection<ProjectRevisions> ProjectRevisions { get; set; }

        public virtual ICollection<Events> Events { get; set; }
    }
}
