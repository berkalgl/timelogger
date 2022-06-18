using System.Collections.Generic;
using Timelogger.Domain.Core.Extentions;

namespace Timelogger.Domain.Projects
{
    public interface IProjectService
    {
        public List<ProjectDTO> GetProjects();
        public ProjectDTO GetProjectById(int id);
        public ProjectDTO AddProject(ProjectDTO projectDTO);
    }
}
