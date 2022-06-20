using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Timelogger.Api.Common;
using Timelogger.Domain.Core.Extentions;
using Timelogger.Domain.Timesheets;

namespace Timelogger.Api.Controllers
{
	[Route("api/[controller]")]
	public class TimesheetsController : Controller
	{
        private readonly ITimesheetService _timesheetService;
        public TimesheetsController(ITimesheetService timesheetService)
		{
            _timesheetService = timesheetService;
		}
        // GET api/timesheets?projectId=
        [HttpGet]
		public ServiceResult<List<TimesheetDTO>> Get(int projectId)
		{
			return new ServiceResult<List<TimesheetDTO>> 
            { 
				Message = ReturnMessages.Success,
				Result = _timesheetService.GetTimesheets(projectId)
            };
        }
        // POST api/timesheets/AddTimesheet
        [Route("AddTimesheet")]
        [HttpPost]
        public ServiceResult<TimesheetDTO> AddTimesheet([FromBody] TimesheetDTO timesheetDTO)
        {
            return new ServiceResult<TimesheetDTO>
            {
                Message = ReturnMessages.AdditionSuccess,
                Result = _timesheetService.AddTimesheet(timesheetDTO)
            };
        }
    }
}
