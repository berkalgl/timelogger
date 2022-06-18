using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Timelogger.Api.Common;
using Timelogger.Domain.Core.Extentions;
using Timelogger.Domain.Projects;

namespace Timelogger.Api.Controllers
{
	[Route("api/[controller]")]
	public class ProjectsController : Controller
	{
        private readonly IProjectService _projectService;
        public ProjectsController(IProjectService projectService)
		{
            _projectService = projectService;
		}

		[HttpGet]
		[Route("hello-world")]
		public string HelloWorld()
		{
			return "Hello Back!";
		}

        // GET api/projects?Offset=0&PageSize=10&Sort.Direction=DESC&Sort.Field=Deadline
        [HttpGet]
		public ServiceResult<List<ProjectDTO>> Get()
		{
			return new ServiceResult<List<ProjectDTO>>
            {
                Message = ReturnMessages.Success,
                Result = _projectService.GetProjects()
            };
        }
        // GET api/projects/GetById
        [HttpGet]
        [Route("GetById")]
        public ServiceResult<ProjectDTO> GetById(int id)
        {
            return new ServiceResult<ProjectDTO>
            {
                Message = ReturnMessages.Success,
                Result = _projectService.GetProjectById(id)
            };
        }
        // Post api/projects/AddProject
        [Route("AddProject")]
        [HttpPost]
        public ServiceResult<ProjectDTO> AddProject([FromBody] ProjectDTO projectDTO)
        {
            return new ServiceResult<ProjectDTO>
            {
                Message = ReturnMessages.AdditionSuccess,
                Result = _projectService.AddProject(projectDTO)
            };
        }
    }
}
