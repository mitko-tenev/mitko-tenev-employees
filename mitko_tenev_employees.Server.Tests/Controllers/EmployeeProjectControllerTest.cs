namespace mitko_tenev_employees.Server.Tests.Controllers
{
    using Microsoft.AspNetCore.Http;
    using mitko_tenev_employees.Server.Controllers;
    using NSubstitute;
    using System.IO;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using mitko_tenev_employees.Server.Services.Interfaces;
    using mitko_tenev_employees.Server.Tests.Utils;
    using Microsoft.AspNetCore.Http.HttpResults;
    using mitko_tenev_employees.Server.Services;
    using NSubstitute.ExceptionExtensions;
    using mitko_tenev_employees.Server.Exceptions;
    using mitko_tenev_employees.Server.Models;

    public class EmployeeProjectControllerTest
    {
        private readonly EmployeeProjectController _sut;
        private readonly ICsvParserService csvParserService;
        private readonly IEmployeeProjectService employeeProjectService;

        public EmployeeProjectControllerTest()
        {
            csvParserService = Substitute.For<ICsvParserService>();
            employeeProjectService = Substitute.For<IEmployeeProjectService>();
            this._sut = new EmployeeProjectController(csvParserService, employeeProjectService);
        }

        [Fact]
        public async Task Upload()
        {
            var formFileMock = FormFileMockFactory.GetFormFileMock(
                """
                EmpID,ProjectID,DateFrom,DateTo
                143,12,2013-11-01,2014-01-05
                """, "employees.csv");

            List<CommonProject> expected = new()
            {
                new CommonProject()
                {
                    FirstEmployeeID = 143,
                    SecondEmployeeID = 152,
                    ProjectID = 12,
                    DaysWorked = 20
                }
            };

            var employeeProjects = new List<EmployeeProject>();

            csvParserService.ParseCsvFile(formFileMock).Returns(employeeProjects);
            employeeProjectService.FindLongestWorkingPair(employeeProjects).Returns(expected);


            var actual = await this._sut.Upload(formFileMock);

            var result = Assert.IsType<OkObjectResult>(actual);

            result.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Upload_Returns_Bad_Request_For_Null_File()
        {
            IFormFile? formFileMock = null;

            var actual = await this._sut.Upload(formFileMock);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);

            badRequestResult.Value.Should().BeEquivalentTo(new { error = "Uploaded CSV file is empty" });
        }

        [Fact]
        public async Task Upload_Returns_Bad_Request_For_Empty_File()
        {
            var formFileMock = FormFileMockFactory.GetFormFileMock("", "test.csv");

            var actual = await this._sut.Upload(formFileMock);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);

            badRequestResult.Value.Should().BeEquivalentTo(new { error = "Uploaded CSV file is empty" });
        }

        [Fact]
        public async Task Upload_Returns_Bad_Request_For_Invalid_Csv()
        {
            var formFileMock = FormFileMockFactory.GetFormFileMock(
                """
                EmpID,ProjectID,DateFrom,DateTo
                143, 12, 2013-11-01, 2014-01-05
                """
                , "test.csv");

            csvParserService.ParseCsvFile(formFileMock).Throws(new InvalidCsvException("Invalid CSV"));

            var actual = await this._sut.Upload(formFileMock);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);

            badRequestResult.Value.Should().BeEquivalentTo(new { error = "Invalid CSV" });
        }

        [Fact]
        public async Task Upload_Returns_Internal_Server_Error()
        {
            var formFileMock = FormFileMockFactory.GetFormFileMock(
                """
                EmpID,ProjectID,DateFrom,DateTo
                143, 12, 2013-11-01, 2014-01-05
                """
                , "test.csv");

            csvParserService.ParseCsvFile(formFileMock).Throws(new Exception("Unknown error"));

            var actual = await this._sut.Upload(formFileMock);

            var objectResult = Assert.IsType<ObjectResult>(actual);

            objectResult.Value.Should().BeEquivalentTo(new { error = "Server error occurred: Unknown error" });
        }
    }
}
