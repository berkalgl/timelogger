using Microsoft.EntityFrameworkCore;
using Timelogger.Infrastructure.Entities;

namespace Timelogger.Infrastructure.Interfaces
{
    public interface IApiContext
    {
        DbSet<Project> Project { get; set; }
        DbSet<Timesheet> Timesheet { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        void Save();
    }
}
