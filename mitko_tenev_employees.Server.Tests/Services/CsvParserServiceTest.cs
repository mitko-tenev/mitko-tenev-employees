namespace mitko_tenev_employees.Server.Tests.Services
{
    using FluentAssertions;
    using mitko_tenev_employees.Server.Exceptions;
    using mitko_tenev_employees.Server.Models;
    using mitko_tenev_employees.Server.Services;
    using mitko_tenev_employees.Server.Tests.Utils;
    using System;
    using System.Collections.Generic;

    public class CsvParserServiceTest
    {
        private readonly CsvParserService _sut;

        public CsvParserServiceTest()
        {
            this._sut = new CsvParserService();
        }

        [Fact]
        public void ParseCsvFile()
        {
            var formFileMock = FormFileMockFactory.GetFormFileMock(
                """
                EmpID,ProjectID,DateFrom,DateTo
                143, 12, 2013-11-01, 2014-01-05
                218, 10, 2012-05-16, NULL
                143, 10, 2009-01-01, 2011-04-27
                """, "employees.csv");

            var expected = new List<EmployeeProject>()
            {
                new EmployeeProject()
                {
                    EmpID = 143,
                    ProjectID = 12,
                    DateFrom = new DateTime(2013, 11, 1),
                    DateTo =  new DateTime(2014, 1, 5)
                },
                new EmployeeProject()
                {
                    EmpID = 218,
                    ProjectID = 10,
                    DateFrom = new DateTime(2012, 5, 16),
                    DateTo =  new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
                },
                new EmployeeProject()
                {
                    EmpID = 143,
                    ProjectID = 10,
                    DateFrom = new DateTime(2009, 1, 1),
                    DateTo =  new DateTime(2011, 4, 27)
                },
            };

            var actual = this._sut.ParseCsvFile(formFileMock);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ParseCsvFile_Should_Throw_Invalid_CSV()
        {
            var formFileMock = FormFileMockFactory.GetFormFileMock(
                """
                EmpID,ProjectID,DateFrom,DateTo
                a
                """, "employees.csv");


            Action act = () => this._sut.ParseCsvFile(formFileMock);

            act.Should()
                .Throw<InvalidCsvException>()
                .WithMessage("Invalid CSV provided");
        }

        [Theory]
        [InlineData("123", 123)]
        [InlineData("   123", 123)]
        [InlineData("   123   ", 123)]
        public void Should_Parse_EmpID_Correctly(string empId, int expectedId)
        {
            var formFileMock = FormFileMockFactory.GetFormFileMock(
                $"""
                EmpID,ProjectID,DateFrom,DateTo
                {empId}, 12, 2013-11-01, 2014-01-05
                """, "employees.csv");

            var expected = new List<EmployeeProject>()
            {
                new EmployeeProject()
                {
                    EmpID = expectedId,
                    ProjectID = 12,
                    DateFrom = new DateTime(2013, 11, 1),
                    DateTo =  new DateTime(2014, 1, 5)
                }
            };

            var actual = this._sut.ParseCsvFile(formFileMock);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("12", 12)]
        [InlineData("   12", 12)]
        [InlineData("   12   ", 12)]
        public void Should_Parse_ProjectID_Correctly(string projectId, int expectedId)
        {
            var formFileMock = FormFileMockFactory.GetFormFileMock(
                $"""
                EmpID,ProjectID,DateFrom,DateTo
                143, {projectId}, 2013-11-01, 2014-01-05
                """, "employees.csv");

            var expected = new List<EmployeeProject>()
            {
                new EmployeeProject()
                {
                    EmpID = 143,
                    ProjectID = expectedId,
                    DateFrom = new DateTime(2013, 11, 1),
                    DateTo =  new DateTime(2014, 1, 5)
                }
            };

            var actual = this._sut.ParseCsvFile(formFileMock);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("2025-04-26", 2025, 4, 26)]
        [InlineData("2025/04/26", 2025, 4, 26)]
        [InlineData("26/04/2025", 2025, 4, 26)]
        [InlineData("26-04-2025", 2025, 4, 26)]
        [InlineData("04/26/2025", 2025, 4, 26)]
        [InlineData("04-26-2025", 2025, 4, 26)]
        public void Should_Parse_DateFrom_Correctly(string dateFrom, int year, int month, int day)
        {
            var expectedDate = new DateTime(year, month, day, 0, 0, 0);

            var formFileMock = FormFileMockFactory.GetFormFileMock(
                $"""
                EmpID,ProjectID,DateFrom,DateTo
                143, 12, {dateFrom}, 2014-01-05
                """, "employees.csv");

            var expected = new List<EmployeeProject>()
            {
                new EmployeeProject()
                {
                    EmpID = 143,
                    ProjectID = 12,
                    DateFrom = expectedDate,
                    DateTo =  new DateTime(2014, 1, 5)
                }
            };

            var actual = this._sut.ParseCsvFile(formFileMock);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("2025-04-26", 2025, 4, 26)]
        [InlineData("2025/04/26", 2025, 4, 26)]
        [InlineData("26/04/2025", 2025, 4, 26)]
        [InlineData("26-04-2025", 2025, 4, 26)]
        [InlineData("04/26/2025", 2025, 4, 26)]
        [InlineData("04-26-2025", 2025, 4, 26)]
        [InlineData("NULL", 0, 0 ,0)]
        public void Should_Parse_DateTo_Correctly(string dateTo, int year, int month, int day)
        {
            var expectedDate = dateTo != "NULL" 
                ? new DateTime(year, month, day, 0, 0, 0) 
                : new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);

            var formFileMock = FormFileMockFactory.GetFormFileMock(
                $"""
                EmpID,ProjectID,DateFrom,DateTo
                143, 12, 2013-11-01, {dateTo}
                """, "employees.csv");

            var expected = new List<EmployeeProject>()
            {
                new EmployeeProject()
                {
                    EmpID = 143,
                    ProjectID = 12,
                    DateFrom =  new DateTime(2013, 11, 1),
                    DateTo = expectedDate
                }
            };

            var actual = this._sut.ParseCsvFile(formFileMock);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
