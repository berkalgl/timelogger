using System;
using System.Collections.Generic;
using System.Linq;
using Timelogger.Domain.Core.Exceptions;
using Timelogger.Domain.Core.Extentions;
using Timelogger.Domain.Core.Interfaces;
using Timelogger.Infrastructure.Entities;

namespace Timelogger.Domain.Timesheets
{
    public class TimesheetService : ITimesheetService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TimesheetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<TimesheetDTO> GetTimesheets()
        {
            var baseQuery = _unitOfWork.TimesheetRepository.Find(i =>!i.IsDeleted);

            return baseQuery.Select(i => new TimesheetDTO { Id = i.Id, ProjectId = i.ProjectId, Comment = i.Comment, StartTime = i.StartTime, EndTime = i.EndTime }).ToList();
        }
        public TimesheetDTO AddTimesheet(TimesheetDTO timesheetDTO)
        {
            IsValid(timesheetDTO);

            // usually id is generated automatically. in this case we need to find last id to increase
            var lastId = _unitOfWork.TimesheetRepository.GetAll().OrderByDescending(i => i.Id).Select(i => i.Id).FirstOrDefault() + 1;

            try
            {
                _unitOfWork.TimesheetRepository.AddAndSave(
                    new Timesheet
                    {
                        Id = lastId,
                        Comment = timesheetDTO.Comment,
                        ProjectId = timesheetDTO.ProjectId,
                        StartTime = timesheetDTO.StartTime,
                        EndTime = timesheetDTO.EndTime
                    });
            }
            catch(Exception ex)
            {
                throw new TimeloggerException("There is something wrong while Adding a Timesheet :" + ex.Message);
            }

            timesheetDTO.Id = lastId;
            return timesheetDTO;
        }
        private void IsValid(TimesheetDTO timesheetDTO)
        {
            if (timesheetDTO.StartTime == DateTime.MinValue)
                throw new TimeloggerException("Start time cannot be empty");

            if (timesheetDTO.EndTime == DateTime.MinValue)
                throw new TimeloggerException("End time time cannot be empty");

            if (timesheetDTO.StartTime > timesheetDTO.EndTime)
                throw new TimeloggerException("Start time cannot be bigger than End time");

            //check if timesheets intersect with each other
            var checkSameTime = _unitOfWork.TimesheetRepository
                .FirstOrDefault(i => i.ProjectId.Equals(timesheetDTO.ProjectId) && !i.IsDeleted
                && !(i.StartTime >= timesheetDTO.EndTime || i.EndTime <= timesheetDTO.StartTime));

            if (checkSameTime != null)
                throw new TimeloggerException("You are trying to insert a date which interects with other");
        }
    }
}
