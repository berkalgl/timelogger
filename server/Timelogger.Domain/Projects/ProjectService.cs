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
        public List<ProjectDTO> GetProjects(string name)
        {
            var baseQuery = _unitOfWork.ProjectRepository.Find(i => (String.IsNullOrEmpty(name) || i.Name.Contains(name)));

            var result = baseQuery
                .Select(i => 
                new ProjectDTO 
                { 
                    Id = i.Id,
                    Name = i.Name, 
                    Deadline = i.Deadline,
                    IsDeleted = i.IsDeleted,
                    TotalHoursWorked = GetTotalHoursWorked(i.Id)
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
        public void DisableProject(int id)
        {
            var project = _unitOfWork.ProjectRepository.GetById(id);

            if (project == null) throw new TimeloggerException("Could not find the project");

            if (project.IsDeleted) throw new TimeloggerException("It is already disabled");

            project.IsDeleted = true;

            _unitOfWork.ProjectRepository.UpdateAndSave(project);

        }
        private double GetTotalHoursWorked(int projectId)
        {
            return Math.Round(_unitOfWork.TimesheetRepository.Find(i => i.ProjectId.Equals(projectId)).Select(i => (i.EndTime - i.StartTime).TotalHours).Sum(), 2);
        }
        private void IsValid(ProjectDTO projectDTO)
        { 
            if (projectDTO == null)
                throw new TimeloggerException("Input project cannot be null or empty");

            if (string.IsNullOrEmpty(projectDTO.Name))
                throw new TimeloggerException("Name cannot be null or empty");

            if (projectDTO.Deadline == DateTime.MinValue)
                throw new TimeloggerException("Deadline cannot be empty");

        }
    }
}
