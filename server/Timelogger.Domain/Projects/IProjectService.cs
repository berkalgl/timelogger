using System.Collections.Generic;

namespace Timelogger.Domain.Projects
{
    public interface IProjectService
    {
        public List<ProjectDTO> GetProjects(string name);
        public ProjectDTO AddProject(ProjectDTO projectDTO);
        public void DisableProject(int id);
    }
}
