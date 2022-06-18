using System;
using Timelogger.Infrastructure.Entities.Base;

namespace Timelogger.Infrastructure.Entities
{
    public class Timesheet : BaseEntity
    {
        //Foreign Key
        public int ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Comment { get; set; }
    }
}
