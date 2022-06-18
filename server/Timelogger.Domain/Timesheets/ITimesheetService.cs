using System.Collections.Generic;
using Timelogger.Domain.Core.Extentions;

namespace Timelogger.Domain.Timesheets
{
    public interface ITimesheetService
    {
        public TimesheetDTO AddTimesheet(TimesheetDTO timesheetDTO);
        public List<TimesheetDTO> GetTimesheets();
    }
}
