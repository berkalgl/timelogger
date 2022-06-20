using NUnit.Framework;
using System.Collections.Generic;
using Timelogger.Domain.Core.Exceptions;
using Timelogger.Domain.Core.Extentions;
using Timelogger.Domain.Projects;
using Timelogger.Domain.Timesheets;

namespace Timelogger.Api.Tests.Domain
{
    [TestFixture]
    public class TimesheetServiceTests : BaseTest
    {
        private TimesheetService _timesheetService;

        [SetUp]
        public void SetUp()
        {
            _timesheetService = new TimesheetService(_mockUnitOfWork.Object);
        }
        [Test]
        public void GetTimesheets_ShouldReturnResult()
        {
            // Arrange
            //Act
            var result = _timesheetService.GetTimesheets(1);

            //Assert
            Assert.AreEqual(result.Count, 2);
            Assert.That(result[0].Id == 1);

        }
        [Test]
        public void AddTimesheet_ShouldAddAndReturnId_WithNonEmptyInput()
        {
            // Arrange
            var timesheetDTO = new TimesheetDTO() 
            { 
                ProjectId = 1,
                Comment = "TimesheetComment",
                StartTime = new System.DateTime(2022, 6, 13, 17, 00, 0),
                EndTime = new System.DateTime(2022, 6, 13, 18, 00, 0)
            };
            //Act
            var returnTimesheetDTO = _timesheetService.AddTimesheet(timesheetDTO);
            //Assert
            Assert.AreEqual(returnTimesheetDTO.Id, 3);
        }
        [Test]
        public void AddTimesheet_ShouldThrowCannotbeEmpty_WithEmptyStartDate()
        {
            // Arrange
            var timesheetDTO = new TimesheetDTO()
            {
                ProjectId = 1,
                Comment = "TimesheetComment",
                EndTime = new System.DateTime(2022, 6, 13, 17, 00, 0)
            };
            //Act
            var ex = Assert.Throws<TimeloggerException>(() => _timesheetService.AddTimesheet(timesheetDTO));
            //Assert
            Assert.That(ex.Message == "Start time cannot be empty");
        }
        [Test]
        public void AddTimesheet_ShouldThrowCannotbeEmpty_WithEmptyEndDate()
        {
            // Arrange
            var timesheetDTO = new TimesheetDTO()
            {
                ProjectId = 1,
                Comment = "TimesheetComment",
                StartTime = new System.DateTime(2022, 6, 13, 17, 00, 0)
            };
            //Act
            var ex = Assert.Throws<TimeloggerException>(() => _timesheetService.AddTimesheet(timesheetDTO));
            //Assert
            Assert.That(ex.Message == "End time time cannot be empty");
        }
        [Test]
        public void AddTimesheet_ShouldThrowBiggerException_WithStartEndDate()
        {
            // Arrange
            var timesheetDTO = new TimesheetDTO()
            {
                ProjectId = 1,
                Comment = "TimesheetComment",
                StartTime = new System.DateTime(2022, 6, 13, 17, 00, 0),
                EndTime = new System.DateTime(2022, 6, 12, 17, 00, 0),
            };
            //Act
            var ex = Assert.Throws<TimeloggerException>(() => _timesheetService.AddTimesheet(timesheetDTO));
            //Assert
            Assert.That(ex.Message == "Start time cannot be bigger than End time");
        }
    }
}
