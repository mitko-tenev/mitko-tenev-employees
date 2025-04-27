namespace mitko_tenev_employees.Server.Services
{
    using mitko_tenev_employees.Server.Models;
    using mitko_tenev_employees.Server.Services.Interfaces;

    public class EmployeeProjectService : IEmployeeProjectService
    {
        public IEnumerable<CommonProject> FindLongestWorkingPair(IEnumerable<EmployeeProject> parsedEmployeeProjects)
        {
            List<CommonProject> commonProjects = CalculateCommonProjects(parsedEmployeeProjects);
            List<CommonProject> longestCommonPairResult = FindLongestWorkingPairHelper(commonProjects);

            return longestCommonPairResult;
        }


        /**
         * Calculates common projects by first grouping all employee projects by project id.
         * Then checking for employees with overlapping working periods.
         * If such overlaps are found, the employees are stored in a result collection.
         */
        private List<CommonProject> CalculateCommonProjects(IEnumerable<EmployeeProject> employeeProjects)
        {
            var result = new List<CommonProject>();

            // Group by ProjectID
            var projectGroups = employeeProjects.GroupBy(e => e.ProjectID);

            foreach (var projectGroup in projectGroups)
            {
                var projectId = projectGroup.Key;
                var employees = projectGroup.ToList();

                // Find all pairs of employees on this project
                for (int i = 0; i < employees.Count; i++)
                {
                    for (int j = i + 1; j < employees.Count; j++)
                    {
                        var emp1 = employees[i];
                        var emp2 = employees[j];

                        // Check if they worked together (overlapping period)
                        var overlapStart = emp1.DateFrom > emp2.DateFrom ? emp1.DateFrom : emp2.DateFrom;
                        var overlapEnd = emp1.DateTo < emp2.DateTo ? emp1.DateTo : emp2.DateTo;

                        var isWorkingPeriodOverlapping = overlapStart <= overlapEnd;

                        if (isWorkingPeriodOverlapping)
                        {
                            var daysWorked = (overlapEnd.Value - overlapStart).Days + 1; // +1 to include both start and end dates

                            if (daysWorked > 0)
                            {
                                result.Add(new CommonProject
                                {
                                    FirstEmployeeID = Math.Min(emp1.EmpID, emp2.EmpID), // Always put lower ID first for consistency
                                    SecondEmployeeID = Math.Max(emp1.EmpID, emp2.EmpID),
                                    ProjectID = projectId,
                                    DaysWorked = daysWorked
                                });
                            }
                        }
                    }
                }
            }

            return result;
        }

        /**
         * Contains logic for finding longes working pair.
         * First it groups the projects by the two employees' ids.
         * Then it calculates the total days worked and sorts the list of projects by total days worked in descending order.
         * Finally if there are such pair groups, it gets the first such group, orders the projects worked 
         * on by the group in descending order and returns the resulting list.
         * If there is no such group, an empty list is returned.
         */
        private List<CommonProject> FindLongestWorkingPairHelper(IEnumerable<CommonProject> commonProjects)
        {
            // Group by employee pairs
            var pairGroups = commonProjects.GroupBy(c => new { c.FirstEmployeeID, c.SecondEmployeeID })
                .Select(g => new
                {
                    EmployeePair = g.Key,
                    TotalDaysWorked = g.Sum(c => c.DaysWorked),
                    Projects = g.ToList()
                })
                .OrderByDescending(g => g.TotalDaysWorked)
                .ToList();

            var result = pairGroups.Any()
                    ? pairGroups
                        .First().Projects
                        .OrderByDescending(p => p.DaysWorked)
                        .ToList()
                    : new List<CommonProject>();

            return result;
        }
    }
}
