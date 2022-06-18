using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Timelogger.Api.Handler;
using Timelogger.Domain.Core.Extentions;
using System.Collections.Generic;
using Timelogger.Infrastructure.Entities;

namespace Timelogger.Api
{
    public class Startup
	{
		private readonly IWebHostEnvironment _environment;
		public IConfigurationRoot Configuration { get; }

		public Startup(IWebHostEnvironment env)
		{
			_environment = env;

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
            // Add framework services.
            services.Register();
			services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("e-conomic interview"));
			services.AddLogging(builder =>
			{
				builder.AddConsole();
				builder.AddDebug();
			});

			services.AddMvc(options => options.EnableEndpointRouting = false);

			if (_environment.IsDevelopment())
			{
				services.AddCors();
			}
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseCors(builder => builder
					.AllowAnyMethod()
					.AllowAnyHeader()
					.SetIsOriginAllowed(origin => true)
					.AllowCredentials());
            }

            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context => await ExceptionHandler.HandleException(context));
            });

            app.UseMvc();

            var serviceScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
			using (var scope = serviceScopeFactory.CreateScope())
			{
				SeedDatabase(scope);
			}
		}

		private static void SeedDatabase(IServiceScope scope)
		{
			var context = scope.ServiceProvider.GetService<ApiContext>();

			var listOfProject = new List<Project>
			{
				new Project{
					Id = 1,
					Name = "e-conomic Interview",
					Deadline = System.DateTime.Now,
                    IsDeleted = false
                },
                new Project{
                    Id = 2,
                    Name = "e-conomic Interview2",
                    Deadline = System.DateTime.Now.AddDays(3),
					IsDeleted = false
                }
            };

            var listOfTimesheet = new List<Timesheet>
            {
                new Timesheet{
                    Id = 1,
					ProjectId = 1,
                    IsDeleted = false,
                    Comment = "e-conomic Interview worked 1",
                    StartTime = new System.DateTime(2022, 6, 12, 14, 30, 0),
					EndTime = new System.DateTime(2022, 6, 12, 15, 30, 0),
                },
                new Timesheet{
                    Id = 2,
                    ProjectId = 1,
					IsDeleted = false,
                    Comment = "e-conomic Interview worked 2",
                    StartTime = new System.DateTime(2022, 6, 13, 17, 00, 0),
                    EndTime = new System.DateTime(2022, 6, 13, 18, 00, 0),
                },
                new Timesheet{
                    Id = 3,
                    ProjectId = 2,
                    IsDeleted = false,
                    Comment = "e-conomic Interview worked 21",
                    StartTime = new System.DateTime(2022, 6, 12, 14, 30, 0),
                    EndTime = new System.DateTime(2022, 6, 12, 15, 30, 0),
                },
                new Timesheet{
                    Id = 4,
                    ProjectId = 2,
                    IsDeleted = false,
                    Comment = "e-conomic Interview worked 22",
                    StartTime = new System.DateTime(2022, 6, 13, 17, 00, 0),
                    EndTime = new System.DateTime(2022, 6, 13, 18, 00, 0),
                }
            };

            context.Project.AddRange(listOfProject);
            context.Timesheet.AddRange(listOfTimesheet);

            context.SaveChanges();
		}
	}
}