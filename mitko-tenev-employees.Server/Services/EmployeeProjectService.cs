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

                        if (overlapStart <= overlapEnd)
                        {
                            var daysWorked = (overlapEnd.Value - overlapStart).Days + 1; // +1 to include both start and end dates

                            if (daysWorked > 0)
                            {
                                result.Add(new CommonProject
                                {
                                    FirstEmployeeId = Math.Min(emp1.EmpID, emp2.EmpID), // Always put lower ID first for consistency
                                    SecondEmployeeId = Math.Max(emp1.EmpID, emp2.EmpID),
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

        private List<CommonProject> FindLongestWorkingPairHelper(IEnumerable<CommonProject> commonProjects)
        {
            // Group by employee pairs
            var pairGroups = commonProjects.GroupBy(c => new { c.FirstEmployeeId, c.SecondEmployeeId })
                .Select(g => new
                {
                    EmployeePair = g.Key,
                    TotalDaysWorked = g.Sum(c => c.DaysWorked),
                    Projects = g.ToList()
                })
                .OrderByDescending(g => g.TotalDaysWorked)
                .ToList();


            return pairGroups.Any() ? pairGroups.First().Projects : new List<CommonProject>();
        }
    }
}
