using Timelogger.Domain.Core.Interfaces;
using Timelogger.Infrastructure;
using Timelogger.Infrastructure.Entities;
using Timelogger.Infrastructure.Interfaces;

namespace Timelogger.Domain.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IApiContext _context;
        IGenericRepository<Project> _projectRepository { get; set; }
        IGenericRepository<Timesheet> _timesheetRepository { get; set; }
        public UnitOfWork(IApiContext context)
        {
            this._context = context;
        }
        public IApiContext Context
        {
            get
            {
                return _context;
            }
        }
        public IGenericRepository<Project> ProjectRepository
        {
            get { return _projectRepository ?? (_projectRepository = new GenericRepository<Project>(_context)); }
        }
        public IGenericRepository<Timesheet> TimesheetRepository
        {
            get { return _timesheetRepository ?? (_timesheetRepository = new GenericRepository<Timesheet>(_context)); }
        }
    }
}
