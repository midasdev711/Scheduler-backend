using System;
using Repository.Interfaces;
using Repository.Models;
using Repository.ViewModel;
using System.Collections.Generic;
using System.Text;

namespace Repository.Services
{
    public class WorkingHourServices : IWorkingHourServices
    {
        WesternSchedulerContext _context;
        public WorkingHourServices(WesternSchedulerContext context)
        {
            _context = context;
        }
        /*
        public List<Projects> GetProjectList()
        {
            return _context.Projects.Include("Employee").Include("ProjectNumber").Include("ProjectRevisions").ToList();
        }

        public Projects GetProjectById(int id)
        {
            return _context.Projects.Where(p => p.EmployeeId == id).Include("Employee").Include("ProjectNumber").Include("ProjectRevisions").FirstOrDefault();
        }
        */
    }
}
