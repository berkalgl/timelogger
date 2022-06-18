using System;
using System.Collections.Generic;
using System.Linq;
using Timelogger.Domain.Core.Exceptions;
using Timelogger.Domain.Core.Extentions;
using Timelogger.Domain.Core.Interfaces;
using Timelogger.Domain.Timesheets;
using Timelogger.Infrastructure.Entities;

namespace Timelogger.Domain.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<ProjectDTO> GetProjects()
        {
            var baseQuery = _unitOfWork.ProjectRepository.Find(i => !i.IsDeleted);

            var result = baseQuery
                .Select(i => 
                new ProjectDTO 
                { 
                    Id = i.Id,
                    Name = i.Name, 
                    Deadline = i.Deadline, 
                    TotalHoursWorked = GetTotalHoursWorked(i.Id), 
                    timesheets = _unitOfWork.TimesheetRepository.Find(t => !t.IsDeleted).Where(t => t.ProjectId.Equals(i.Id)).Select(i =>
                        new TimesheetDTO 
                        { 
                            Id = i.Id, 
                            ProjectId = i.ProjectId, 
                            Comment = i.Comment, 
                            StartTime = i.StartTime, 
                            EndTime = i.EndTime }).ToList()
                    }).ToList();

            return result;
}
        public ProjectDTO AddProject(ProjectDTO projectDTO)
        {
            IsValid(projectDTO);

            // usually id is generated automatically. in this case we need to find last id to increase
            var lastId = _unitOfWork.ProjectRepository.GetAll().OrderByDescending(i => i.Id).Select(i => i.Id).FirstOrDefault() + 1;

            try
            {
                _unitOfWork.ProjectRepository.AddAndSave(
                    new Project
                    {
                        Id = lastId,
                        Name = projectDTO.Name,
                        Deadline = projectDTO.Deadline
                    });
            }
            catch (Exception ex)
            {
                throw new TimeloggerException("There is something wrong while Adding a Project :" + ex.Message);
            }

            projectDTO.Id = lastId;
            return projectDTO;

        }
        public ProjectDTO GetProjectById(int id)
        {
            //We can create a Static Language File for hard coded strings.
            if (id == 0)
                throw new TimeloggerException("Id cannot be zero");

            var project = _unitOfWork.ProjectRepository.GetById(id);

            if (project is null)
                throw new TimeloggerException("Could not be found !");

            return new ProjectDTO { Id = project.Id, Name = project.Name };

        }
        private double GetTotalHoursWorked(int projectId)
        {
            return _unitOfWork.TimesheetRepository.Find(i => i.ProjectId.Equals(projectId)).Select(i => (i.EndTime - i.StartTime).TotalHours).Sum();
        }
        private void IsValid(ProjectDTO projectDTO)
        { 
            if (string.IsNullOrEmpty(projectDTO.Name))
                throw new TimeloggerException("Name cannot be null or empty");

            if (projectDTO.Deadline == DateTime.MinValue)
                throw new TimeloggerException("Deadline cannot be empty");

        }
    }
}
