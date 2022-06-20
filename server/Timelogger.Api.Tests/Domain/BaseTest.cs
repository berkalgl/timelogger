using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Timelogger.Domain.Core.Interfaces;
using Timelogger.Domain.Timesheets;
using Timelogger.Infrastructure.Entities;
using Timelogger.Infrastructure.Interfaces;

namespace Timelogger.Api.Tests.Domain
{
    public class BaseTest
    {
        protected readonly Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

        [SetUp]
        public void CommonSetUp()
        {
            #region Base Setup
            var projectRepository = new Mock<IGenericRepository<Project>>();
            var timesheetRepository = new Mock<IGenericRepository<Timesheet>>();
            var apiContext = new Mock<IApiContext>();   

            List<Project> projects = new List<Project>()
           {
                new Project{
                    Id = 1,
                    Name = "e-conomic Interview",
                    Deadline = System.DateTime.Now
                },
                new Project{
                    Id = 2,
                    Name = "e-conomic Interview2",
                    Deadline = System.DateTime.Now.AddDays(3)
                }
           };
            List<Timesheet> timesheets = new List<Timesheet>()
            {
                new Timesheet{
                    Id = 1,
                    ProjectId = 1,
                    IsDeleted = false,
                    Comment = "e-conomic Interview worked 1",
                    StartTime = new DateTime(2022, 6, 12, 14, 30, 0),
                    EndTime = new DateTime(2022, 6, 12, 15, 30, 0),
                },
                new Timesheet{
                    Id = 2,
                    ProjectId = 1,
                    IsDeleted = false,
                    Comment = "e-conomic Interview worked 2",
                    StartTime = new DateTime(2022, 6, 13, 17, 00, 0),
                    EndTime = new DateTime(2022, 6, 13, 18, 00, 0),
                }
            };

            var mockProject = MockDbSet(projects);
            var mockTimesheet = MockDbSet(timesheets);
            #endregion

            #region Context Setup
            apiContext.Setup(_ => _.Project).Returns(mockProject);
            apiContext.Setup(_ => _.Timesheet).Returns(mockTimesheet);
            _mockUnitOfWork.Setup(_ => _.Context).Returns(apiContext.Object);
            #endregion

            #region ProjectRepository Setup
            projectRepository.Setup(_ => _.Find(i => !i.IsDeleted && (String.IsNullOrEmpty(null) || i.Name.Contains(null)))).Returns(projects);
            projectRepository.Setup(_ => _.GetAll()).Returns(projects);
            projectRepository.Setup(_ => _.AddAndSave(new Project() 
            { 
                Id = 3,
                Name = "ProjectName", 
                Deadline = new System.DateTime(2022, 6, 13, 17, 00, 0)
            })).Verifiable();
            _mockUnitOfWork.Setup(_ => _.ProjectRepository).Returns(projectRepository.Object);

            #endregion

            #region TimesheetRepository
            timesheetRepository.Setup(_ => _.AddAndSave(new Timesheet()
            {
                ProjectId = 1,
                Comment = "TimesheetComment",
                StartTime = new DateTime(2022, 6, 13, 17, 00, 0),
                EndTime = new DateTime(2022, 6, 13, 18, 00, 0)
            })).Verifiable();
            timesheetRepository.Setup(_ => _.GetAll()).Returns(timesheets);
            timesheetRepository.Setup(_ => _.Find(i => !i.IsDeleted && i.ProjectId.Equals(1))).Returns(timesheets);
            timesheetRepository.Setup(_ => _.Find(i => i.ProjectId.Equals(1))).Returns(timesheets);
            _mockUnitOfWork.Setup(_ => _.TimesheetRepository).Returns(timesheetRepository.Object);
            #endregion

        }
        private static DbSet<T> MockDbSet<T>(IEnumerable<T> list) where T : class, new()
        {
            IQueryable<T> queryableList = list.AsQueryable();
            Mock<DbSet<T>> dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Provider).Returns(queryableList.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Expression).Returns(queryableList.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(queryableList.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(queryableList.GetEnumerator());
            return dbSetMock.Object;
        }

    }
}
