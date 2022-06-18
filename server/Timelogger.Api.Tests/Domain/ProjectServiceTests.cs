using NUnit.Framework;
using System;
using Timelogger.Domain.Core.Exceptions;
using Timelogger.Domain.Core.Extentions;
using Timelogger.Domain.Projects;

namespace Timelogger.Api.Tests.Domain
{
    [TestFixture]
    public class ProjectServiceTests : BaseTest
    {
        private ProjectService _projectService;

        [SetUp]
        public void SetUp()
        {
            _projectService = new ProjectService(_mockUnitOfWork.Object);
        }
        [Test]
        public void GetProject_ShouldReturnResult()
        {
            // Arrange
            //Act
            var result = _projectService.GetProjects();

            //Assert
            Assert.AreEqual(result.Count, 2);
            Assert.That(result[0].TotalHoursWorked == 2);

        }
        [Test]
        public void GetProjectById_ShouldReturnId_1_WithId_1()
        {
            // Arrange
            int id = 1;
            //Act
            var project = _projectService.GetProjectById(id);

            //Assert
            Assert.AreEqual(project.Id, 1);

        }
        [Test]
        public void GetProjectById_ShouldIdCannotBeZero_WithId_0()
        {
            // Arrange
            int id = 0;
            //Act
            var ex = Assert.Throws<TimeloggerException>(() => _projectService.GetProjectById(id));

            //Assert
            Assert.That(ex.Message == "Id cannot be zero");

        }
        [Test]
        public void GetProjectById_ShouldReturnCouldNotBeFound_WithNonExistedId()
        {
            // Arrange
            int id = -1;
            //Act
            var ex = Assert.Throws<TimeloggerException>(() => _projectService.GetProjectById(id));

            //Assert
            Assert.That(ex.Message == "Could not be found !");
        }
        [Test]
        public void AddProject_ShouldAddAndReturnId_WithNonEmptyInput()
        {
            // Arrange
            var projectDTO = new ProjectDTO() { Name = "ProjectName", Deadline = new System.DateTime(2022, 6, 13, 17, 00, 0) };
            //Act
            var returnProjectDTO = _projectService.AddProject(projectDTO);
            //Assert
            Assert.AreEqual(returnProjectDTO.Id, 3);
        }
        [Test]
        public void AddProject_ShouldThrowsNameException_WithEmptyNameInput()
        {
            // Arrange
            var projectDTO = new ProjectDTO() { Deadline = new DateTime(2022, 6, 13, 17, 00, 0) };
            //Act
            var ex = Assert.Throws<TimeloggerException>(() => _projectService.AddProject(projectDTO));

            //Assert
            Assert.That(ex.Message == "Name cannot be null or empty");
        }
        [Test]
        public void AddProject_ShouldThrowsDeadlineException_WithEmptyDeadlineInput()
        {
            // Arrange
            var projectDTO = new ProjectDTO() { Name = "ProjectName" };
            //Act
            var ex = Assert.Throws<TimeloggerException>(() => _projectService.AddProject(projectDTO));

            //Assert
            Assert.That(ex.Message == "Deadline cannot be empty");
        }
    }
}
