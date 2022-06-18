using System;

namespace Timelogger.Domain.Timesheets
{
    public class TimesheetDTO
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Comment { get; set; }
    }
}
