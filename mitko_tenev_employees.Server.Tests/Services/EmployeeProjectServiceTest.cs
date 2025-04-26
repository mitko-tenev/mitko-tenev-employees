namespace mitko_tenev_employees.Server.Tests.Services
{
    using FluentAssertions;
    using mitko_tenev_employees.Server.Models;
    using mitko_tenev_employees.Server.Services;
    using System;
    using System.Collections.Generic;

    public class EmployeeProjectServiceTest
    {
        private readonly EmployeeProjectService _sut;

        public EmployeeProjectServiceTest()
        {
            this._sut = new EmployeeProjectService();
        }

        [Fact]
        public void FindLongestWorkingPair()
        {
            List<EmployeeProject> employeeProjects = new()
            {
                new EmployeeProject()
                {
                    EmpID = 143,
                    ProjectID = 12,
                    DateFrom = new DateTime(2024, 1, 1, 0, 0, 0),
                    DateTo = new DateTime(2025, 1, 1, 0, 0, 0),
                },
                new EmployeeProject()
                {
                    EmpID = 100,
                    ProjectID = 12,
                    DateFrom = new DateTime(2024, 5, 1, 0, 0, 0),
                    DateTo = new DateTime(2024, 12, 1, 0, 0, 0),
                },
                new EmployeeProject()
                {
                    EmpID = 143,
                    ProjectID = 10,
                    DateFrom = new DateTime(2023, 1, 1, 0, 0, 0),
                    DateTo = new DateTime(2023, 3, 1, 0, 0, 0),
                },
                new EmployeeProject()
                {
                    EmpID = 100,
                    ProjectID = 10,
                    DateFrom = new DateTime(2023, 1, 15, 0, 0, 0),
                    DateTo = new DateTime(2023, 2, 1, 0, 0, 0),
                },
                new EmployeeProject()
                {
                    EmpID = 155,
                    ProjectID = 18,
                    DateFrom = new DateTime(2024, 1, 15, 0, 0, 0),
                    DateTo = new DateTime(2024, 1, 31, 0, 0, 0),
                },
            };

            List<CommonProject> expected = new()
            {
                new CommonProject()
                {
                    FirstEmployeeID = 100,
                    SecondEmployeeID = 143,
                    ProjectID = 12,
                    DaysWorked = 215
                },
                new CommonProject()
                {
                    FirstEmployeeID = 100,
                    SecondEmployeeID = 143,
                    ProjectID = 10,
                    DaysWorked = 18
                },
            };

            var actual = this._sut.FindLongestWorkingPair(employeeProjects);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
