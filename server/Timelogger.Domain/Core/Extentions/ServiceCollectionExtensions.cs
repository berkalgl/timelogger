using Microsoft.Extensions.DependencyInjection;
using Timelogger.Domain.Core.Interfaces;
using Timelogger.Domain.Core.Repository;
using Timelogger.Domain.Projects;
using Timelogger.Domain.Timesheets;
using Timelogger.Infrastructure.Interfaces;

namespace Timelogger.Domain.Core.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<IApiContext, ApiContext>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<ITimesheetService, TimesheetService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
