using Timelogger.Infrastructure.Entities;
using Timelogger.Infrastructure.Interfaces;

namespace Timelogger.Domain.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IApiContext Context { get; }
        IGenericRepository<Project> ProjectRepository { get; }
        IGenericRepository<Timesheet> TimesheetRepository { get; }
    }
}
