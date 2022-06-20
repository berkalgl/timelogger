using System;
using System.Collections.Generic;

namespace Timelogger.Domain.Projects
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public double TotalHoursWorked { get; set; }
    }
}
