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

        // GET api/projects?name=a
        [HttpGet]
		public ServiceResult<List<ProjectDTO>> Get(string name)
		{
			return new ServiceResult<List<ProjectDTO>>
            {
                Message = ReturnMessages.Success,
                Result = _projectService.GetProjects(name)
            };
        }
        // POST api/projects/AddProject
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
